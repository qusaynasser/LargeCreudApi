namespace LargeCreudApi.Model
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}
