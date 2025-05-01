namespace OmniPyme.Web.Data.Entities
{
    public class RolePermission
    {
        public int Roleid { get; set; }

        public Role Role { get; set; }


        public int permissionid  { get; set; }
        public Permission Permission { get; set; }
    }
}
