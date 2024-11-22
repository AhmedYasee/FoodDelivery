import pandas as pd
import pickle
from sklearn.preprocessing import MinMaxScaler
from sklearn.cluster import KMeans
from sklearn.linear_model import LogisticRegression
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

# Fetch customer data
query = """
SELECT u.UserName AS CustomerName, 
       SUM(d.Quantity * d.Price) AS TotalRevenue,
       COUNT(DISTINCT h.Id) AS OrderCount,
       DATEDIFF(DAY, MAX(h.OrderDate), GETDATE()) AS Recency,
       AVG(d.Price * d.Quantity) AS AverageOrderValue
FROM AspNetUsers u
JOIN OrderHeaders h ON u.Id = h.AppUserId
JOIN OrderDetails d ON h.Id = d.OrderHeaderId
GROUP BY u.UserName;
"""

conn = pyodbc.connect(connection_string)
data = pd.read_sql(query, conn)

if data.empty:
    print("No data available for advanced analysis.")
    exit()

# Normalize the features
features = data[['TotalRevenue', 'OrderCount', 'Recency']]
normalized_features = scaler.transform(features)

# Predict clusters
data['Segment'] = kmeans.predict(normalized_features)

# Calculate Customer Lifetime Value (CLV)
data['LifetimeValue'] = data['TotalRevenue'] * data['OrderCount']

# Churn Prediction
data['ChurnRisk'] = (data['Recency'] > data['Recency'].mean()).astype(int)

# Save results to the database
cursor = conn.cursor()
cursor.execute("DELETE FROM CustomerSegmentationResults")

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

print("Advanced segmentation results saved successfully!")
