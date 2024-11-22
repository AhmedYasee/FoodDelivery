import pandas as pd
import pickle
import pyodbc

# Load the trained scaler and model
scaler_path = "C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\scaler.pkl"
model_path = "C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\kmeans_model.pkl"

with open(scaler_path, "rb") as scaler_file:
    scaler = pickle.load(scaler_file)

with open(model_path, "rb") as model_file:
    kmeans = pickle.load(model_file)

# Database connection settings
connection_string = (
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=.;"
    "DATABASE=FoodDel;"
    "Trusted_Connection=yes;"
    "Encrypt=no;"
)

# SQL query to fetch customer data
query = """
SELECT u.UserName AS CustomerName, 
       SUM(d.Quantity * d.Price) AS TotalRevenue,
       COUNT(DISTINCT h.Id) AS OrderCount,
       DATEDIFF(DAY, MAX(h.OrderDate), GETDATE()) AS Recency
FROM AspNetUsers u
LEFT JOIN OrderHeaders h ON u.Id = h.AppUserId
LEFT JOIN OrderDetails d ON h.Id = d.OrderHeaderId
WHERE u.UserName NOT LIKE '%Admin%'  -- Exclude admin users
GROUP BY u.UserName;
"""

conn = pyodbc.connect(connection_string)
data = pd.read_sql(query, conn)

# Fill missing values with defaults
data.fillna(0, inplace=True)

# Ensure proper data types
data['TotalRevenue'] = data['TotalRevenue'].astype(float).round(2)
data['OrderCount'] = data['OrderCount'].astype(int)
data['Recency'] = data['Recency'].astype(int)

# Check for existing customer data
if data.empty:
    print("No data available for segmentation.")
    conn.close()
    exit()

# Normalize and predict clusters
if len(data) >= 3:  # Ensure enough samples for clustering
    features = data[['TotalRevenue', 'OrderCount', 'Recency']]
    normalized_features = scaler.transform(features)
    data['Segment'] = kmeans.predict(normalized_features)

    # Map clusters to human-readable names
    segment_names = {0: "High Value", 1: "Medium Value", 2: "Low Value"}
    data['Segment'] = data['Segment'].map(segment_names)

    # Calculate Customer Lifetime Value (CLV)
    data['LifetimeValue'] = data['TotalRevenue'] * data['OrderCount']
    data['LifetimeValue'] = data['LifetimeValue'].astype(float).round(2)

    # Churn Prediction
    data['ChurnRisk'] = (data['Recency'] > data['Recency'].mean()).astype(int)
    data['ChurnRisk'] = data['ChurnRisk'].map({1: "High Risk", 0: "Low Risk"})

# Save results to the database
cursor = conn.cursor()
cursor.execute("DELETE FROM CustomerSegmentationResults")  # Clear existing records

for _, row in data.iterrows():
    cursor.execute(
        """
        INSERT INTO CustomerSegmentationResults (CustomerName, TotalRevenue, OrderCount, Recency, Segment, LifetimeValue, ChurnRisk)
        VALUES (?, ?, ?, ?, ?, ?, ?)
        """,
        row['CustomerName'], row['TotalRevenue'], row['OrderCount'], row['Recency'], row['Segment'], row['LifetimeValue'], row['ChurnRisk']
    )

conn.commit()
conn.close()

print("Segmentation results saved successfully!")
