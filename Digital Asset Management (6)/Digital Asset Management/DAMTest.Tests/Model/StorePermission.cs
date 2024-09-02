public class StorePermission
{
    public int UserId { get; set; }
    public int StoreId { get; set; }
    public PermissionType Permission { get; set; }

    public StorePermission(int userId, int storeId, PermissionType permission)
    {
        StoreId = storeId;
        UserId = userId;
        Permission = permission;
    }

    public StorePermission(User user,IStore store, PermissionType permission)
    {
        StoreId = store.Id;
        UserId = user.Id;
        Permission = permission;
    }

}