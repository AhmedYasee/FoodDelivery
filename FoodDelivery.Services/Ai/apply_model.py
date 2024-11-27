import os
import pandas as pd
import pickle
from sklearn.preprocessing import MinMaxScaler
from sklearn.cluster import KMeans
from sqlalchemy import create_engine

# Database connection using SQLAlchemy
connection_string = "mssql+pyodbc://:@./FoodDel?driver=ODBC+Driver+17+for+SQL+Server"
engine = create_engine(connection_string)

# Query to fetch customer data
query = """
SELECT 
    u.UserName AS CustomerName,
    SUM(d.Quantity * d.Price) AS TotalRevenue,
    COUNT(DISTINCT h.Id) AS OrderCount,
    DATEDIFF(DAY, MAX(h.OrderDate), GETDATE()) AS Recency
FROM AspNetUsers u
JOIN OrderHeaders h ON u.Id = h.AppUserId
JOIN OrderDetails d ON h.Id = d.OrderHeaderId
GROUP BY u.UserName
"""

# Fetch data
data = pd.read_sql(query, engine)

# Check if there is sufficient data
if data.shape[0] < 3:  # Less than 3 rows
    raise ValueError("Not enough data for clustering. At least 3 samples are required.")

# Normalize the data for segmentation
scaler = MinMaxScaler()
features = data[['TotalRevenue', 'OrderCount', 'Recency']]
normalized_features = scaler.fit_transform(features)

# Dynamically determine the number of clusters for segmentation
n_clusters = min(3, normalized_features.shape[0])
kmeans = KMeans(n_clusters=n_clusters, random_state=42)
data['Segment'] = kmeans.fit_predict(normalized_features)

# Map Segment values to readable labels
segment_labels = {0: 'Low Value', 1: 'Medium Value', 2: 'High Value'}
data['Segment'] = data['Segment'].map(segment_labels)

# Calculate Lifetime Value
data['LifetimeValue'] = data['TotalRevenue'] * data['OrderCount']

# Predict Churn Risk based on Recency
data['ChurnRisk'] = data['Recency'].apply(lambda x: "High Risk" if x > 4 else "Low Risk")

# Save segmentation results back to the database
data.to_sql("CustomerSegmentationResults", con=engine, if_exists="replace", index=False)

# Save results to Excel
output_file_path = "C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\segmentation_results.xlsx"

try:
    data.to_excel(output_file_path, index=False)
    print(f"Results saved successfully to {output_file_path}")
except PermissionError:
    print(f"Permission denied: Unable to write to {output_file_path}. Please close the file if open.")
    alt_path = "C:\\Users\\yasein\\Desktop\\segmentation_results.xlsx"
    data.to_excel(alt_path, index=False)
    print(f"Results saved to an alternative location: {alt_path}")
