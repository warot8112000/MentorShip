namespace DAMTest.Tests;
public class UserTest
{
    public User InitUserData(){
    var user = new User(1,"John");
    var drive1 = new Drive(1,"Drive1");
    var drive2 = new Drive(2,"Drive2");
    user.AddDrive(drive1);
    user.AddDrive(drive2);
    return user;
    }
     
    [Fact]
    public void UserCanOwnMutipleDrives(){
        var user = new User(1,"John");
        var drive3 = new Drive(3,"Drive0");
        user.AddDrive(drive3);
        Assert.Equal(1, user.Drives.Count);
    }

    [Fact]
    public void UserCannotOwnSameDriveTwice(){
        var user = new User(1,"John");
        var drive1 = new Drive(1,"Drive1");
        user.AddDrive(drive1);
        Assert.Throws<InvalidOperationException>(() => user.AddDrive(drive1));
    }

     [Fact]
    public void AddDrive_UserGetsAdminDrivePermission(){
        User user = InitUserData();
        var drive3 = new Drive(3,"Drive3");
        user.AddDrive(drive3);
        Assert.True(user.HasAdminPermission(drive3.Id));
    }

    [Fact]
    public void UserGrantPermission(){
        User user = InitUserData();
        User guest = new User(2,"Jane");
        var drive3 = new Drive(3,"Drive3");
        user.AddDrive(drive3);
        user.GrantPermission(guest, drive3, PermissionType.Reader);
        StorePermission? result = user.StorePermissions.FirstOrDefault
        (x => x.UserId == guest.Id && x.StoreId == drive3.Id);

        Assert.NotNull(result); //Guest has permission
        Assert.Equal(PermissionType.Reader, result.Permission); 
    }
}