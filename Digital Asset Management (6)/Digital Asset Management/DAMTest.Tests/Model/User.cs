using DAMTest.Tests;

public class User
{
    public int Id;
    public string Name;

    public List<Drive> Drives = new List<Drive>();
    public List<StorePermission> StorePermissions = new List<StorePermission>();

    public User(int id, string name)
    {
        this.Id = id;
        Name = name;
    }

    public void AddDrive(Drive drive1)
    {
        if (Drives.Contains(drive1))
        {
            throw new InvalidOperationException("User already owns this drive.");
        }
        Drives.Add(drive1);

        StorePermissions.Add(new StorePermission(this.Id, drive1.Id, PermissionType.Admin));
    }


    public void GrantPermission(User user, IStore store, PermissionType permissionType)
    {
        if (user.HasAdminPermission(store.Id))
        {
            throw new InvalidOperationException("User already owns this store.");
        }    
        ApplyPermissionRecursively(user, store, permissionType);              
    }

    private void ApplyPermissionRecursively(User user, IStore store, PermissionType permissionType)
    {
    
        StorePermissions.Add(new StorePermission(user.Id, store.Id, permissionType));
        if (store.Children != null && store.Children.Any())
        {
            foreach (var child in store.Children)
            {
                ApplyPermissionRecursively(user, child, permissionType);
            }
        }
    }


    public bool HasAdminPermission(int Id)
    {
        return StorePermissions.Any(x => x.StoreId == Id && x.Permission == PermissionType.Admin);
    }

    
}
