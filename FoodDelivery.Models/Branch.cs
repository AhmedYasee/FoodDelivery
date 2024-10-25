namespace FoodDelivery.Models
{
    public class Branch
    {
        public int Id { get; set; }  // Primary key

        public string BranchName { get; set; }  // Branch name - Required

        public string Street { get; set; }  // Required

        public string City { get; set; }  // Required

        public string Country { get; set; }  // Required

        public string State { get; set; }  // Nullable

        public string ZipCode { get; set; }  // Nullable

        public string Phone { get; set; }  // Required

        public string Email { get; set; }  // Nullable

        public string TaxID { get; set; }  // Nullable

        public string CompanyRegistry { get; set; }  // Nullable

        public virtual ApplicationUser Manager { get; set; }  // Navigation property for the manager

        // public ICollection<Warehouse> Warehouses { get; set; }  // Branch can have multiple warehouses
    }
}
