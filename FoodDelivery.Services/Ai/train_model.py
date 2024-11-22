import pandas as pd
from sklearn.preprocessing import MinMaxScaler
from sklearn.cluster import KMeans
import pickle
from datetime import datetime

# Path to the uploaded dataset
dataset_path = "C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\customer_data.xlsx"

# Load the dataset
data = pd.read_excel(dataset_path)

# Calculate total price for each item
data['TotalPrice'] = data['Quantity'] * data['UnitPrice']

# Aggregate data to compute TotalRevenue, OrderCount, and Recency
aggregated_data = data.groupby('CustomerID').agg(
    TotalRevenue=('TotalPrice', 'sum'),
    OrderCount=('InvoiceNo', 'nunique'),
    LastPurchase=('InvoiceDate', 'max')
).reset_index()

# Calculate Recency (days since last purchase)
current_date = datetime.now()
aggregated_data['Recency'] = (current_date - aggregated_data['LastPurchase']).dt.days

# Normalize data
scaler = MinMaxScaler()
features = aggregated_data[['TotalRevenue', 'OrderCount', 'Recency']]
normalized_features = scaler.fit_transform(features)

# Train the K-Means model
kmeans = KMeans(n_clusters=3, random_state=42)
kmeans.fit(normalized_features)

# Save the scaler and model to disk
scaler_path = "C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\scaler.pkl"
model_path = "C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\kmeans_model.pkl"

with open(scaler_path, "wb") as scaler_file:
    pickle.dump(scaler, scaler_file)

with open(model_path, "wb") as model_file:
    pickle.dump(kmeans, model_file)

print("Model trained and saved successfully!")
