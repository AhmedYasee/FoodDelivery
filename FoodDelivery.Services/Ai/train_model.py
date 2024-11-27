import pandas as pd
import pickle
from sklearn.preprocessing import MinMaxScaler
from sklearn.cluster import KMeans
from datetime import datetime

# Path to the uploaded dataset
dataset_path = "C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\customer_data.xlsx"

# Load the dataset
data = pd.read_excel(dataset_path)

# Ensure the dataset contains the required columns
required_columns = ['InvoiceNo', 'StockCode', 'Description', 'Quantity', 'InvoiceDate', 'UnitPrice', 'CustomerID']
if not all(col in data.columns for col in required_columns):
    raise ValueError("Dataset is missing required columns. Please include all required columns: " + ", ".join(required_columns))

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

# Apply the trained model to the dataset
aggregated_data['Segment'] = kmeans.predict(normalized_features)

# Map clusters to human-readable names
segment_names = {0: "Low Value", 1: "Medium Value", 2: "High Value"}
aggregated_data['Segment'] = aggregated_data['Segment'].map(segment_names)

# Save the results to an Excel file
output_file_path = "C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\segmentation_results.xlsx"
aggregated_data.to_excel(output_file_path, index=False)

print("Model trained and applied to dataset successfully!")
print(f"Results saved to {output_file_path}")
