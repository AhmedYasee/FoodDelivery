import os
import pandas as pd
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

# Normalize the data
scaler = MinMaxScaler()
features = data[['TotalRevenue', 'OrderCount', 'Recency']]
normalized_features = scaler.fit_transform(features)

# Dynamically determine the number of clusters
n_clusters = min(3, normalized_features.shape[0])
kmeans = KMeans(n_clusters=n_clusters, random_state=42)
data['Segment'] = kmeans.fit_predict(normalized_features)

# Calculate Lifetime Value (example formula) and Churn Risk
data['LifetimeValue'] = data['TotalRevenue'] / data['OrderCount']  # Example calculation
data['ChurnRisk'] = data['Recency'].apply(lambda x: 1 if x > 4 else 0)  # High risk if recency > 30 days

# Map Segment values to readable labels
segment_labels = {0: 'Low Value', 1: 'Medium Value', 2: 'High Value'}
data['Segment'] = data['Segment'].map(segment_labels)

# Save segmentation results back to the database
data.to_sql("CustomerSegmentationResults", con=engine, if_exists="replace", index=False)
print("Customer segmentation process completed successfully!")
