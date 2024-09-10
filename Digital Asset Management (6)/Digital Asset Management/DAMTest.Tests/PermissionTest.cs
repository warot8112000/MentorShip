using DAMTest; // Add the appropriate using directive for the namespace that contains the 'User' class

namespace DAMTest.Tests;

public class UnitTest1
{
    public User InitUserData(){
    var user = new User(1,"John");
    var drive1 = new Drive(1,"Drive1");
    var drive2 = new Drive(2,"Drive2");
    user.AddDrive(drive1);
    user.AddDrive(drive2);
    return user;
    }

    
//Permissions

    [Fact]
    public void UserHasPermission(){
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


//Check guest has drive permission 
    [Fact]
    public void UserHasAdminDrivePermission(){
        User user = InitUserData();
        User guest = new User(2,"Jane");
        var drive1 = new Drive(1,"Drive1");
        user.AddDrive(drive1);

        user.GrantPermission(guest, drive1, PermissionType.Admin);
        StorePermission? result = user.StorePermissions.FirstOrDefault
        (x => x.UserId == guest.Id && x.StoreId == drive1.Id);

        Assert.NotNull(result); //Guest has permission
        Assert.Equal(PermissionType.Admin, result.Permission); 
    }
    
    [Fact]
    public void UserHasReaderDrivePermission(){
        User user = InitUserData();
        User guest = new User(2,"Jane");
        var drive1 = new Drive(1,"Drive1");
        user.AddDrive(drive1);

        user.GrantPermission(guest, drive1, PermissionType.Reader);
        StorePermission? result = user.StorePermissions.FirstOrDefault
        (x => x.UserId == guest.Id && x.StoreId == drive1.Id);

        Assert.NotNull(result);
        Assert.Equal(PermissionType.Reader, result.Permission); 
    }

    [Fact]
    public void UserHasContributorDrivePermission(){
        User user = InitUserData();
        User guest = new User(2,"Jane");
        var drive1 = new Drive(1,"Drive1");
        user.AddDrive(drive1);

        user.GrantPermission(guest, drive1, PermissionType.Contributor);
        StorePermission? result = user.StorePermissions.FirstOrDefault
        (x => x.UserId == guest.Id && x.StoreId == drive1.Id);

        Assert.NotNull(result);
        Assert.Equal(PermissionType.Contributor, result.Permission); //Guest has contributor permission
    }

    [Fact]
    public void UserHasFolderPermission(){
        User user = InitUserData();
        User guest = new User(2,"Jane");
        var drive3 = new Drive(3,"Drive3");
        var folder1 = new Folder(1,"Folder1");
        drive3.AddFolder(folder1);
        user.AddDrive(drive3);

        user.GrantPermission(guest, folder1, PermissionType.Reader);
        StorePermission? result = user.StorePermissions.FirstOrDefault
        (x => x.UserId == guest.Id && x.StoreId == folder1.Id);

        Assert.NotNull(result); //Guest has permission
        Assert.Equal(PermissionType.Reader, result.Permission); 
    }

    [Fact]
    public void UserHasFilePermission(){
        User user = InitUserData();
        User guest = new User(2,"Jane");
        var drive3 = new Drive(3,"Drive3");
        var file1 = new File(1,"File1");
        drive3.AddFile(file1);
        user.AddDrive(drive3);

        user.GrantPermission(guest, file1, PermissionType.Reader);
        StorePermission? result = user.StorePermissions.FirstOrDefault
        (x => x.UserId == guest.Id && x.StoreId == file1.Id);

        Assert.NotNull(result); //Guest has permission
        Assert.Equal(PermissionType.Reader, result.Permission); 
    }

    [Fact]
    public void FolderIsAddedAsChildOfDrive(){
        User user = InitUserData();
        var drive3 = new Drive(3,"Drive3");
        var folder1 = new Folder(1,"Folder1");
        var folder2 = new Folder(2,"Folder2");
        drive3.AddFolder(folder1);
        user.AddDrive(drive3);

        Assert.Equal(drive3, folder1.Parent);
        Assert.Contains(folder1, drive3.Children);
    }

    [Fact]
    public void FolderIsAddedAsChildOfFolder(){
        User user = InitUserData();
        var drive3 = new Drive(3,"Drive3");
        var folder1 = new Folder(1,"Folder1");
        var folder2 = new Folder(2,"Folder2");
        drive3.AddFolder(folder1);
        folder1.AddFolder(folder2);
        user.AddDrive(drive3);

        Assert.Equal(folder1, folder2.Parent);
        Assert.Contains(folder2, folder1.Children);
    }

    [Fact]
    public void FileIsAddedAsChildOfFolder(){
        User user = InitUserData();
        var drive3 = new Drive(3,"Drive3");
        var folder1 = new Folder(1,"Folder1");
        var file1 = new File(1,"File1");
        drive3.AddFolder(folder1);
        folder1.AddFile(file1);
        user.AddDrive(drive3);

        Assert.Equal(folder1, file1.Parent);
        Assert.Contains(file1, folder1.Children);
    }

    [Fact]
    public void FileIsAddedAsChildOfDrive(){
        User user = InitUserData();
        var drive3 = new Drive(3,"Drive3");
        var file1 = new File(1,"File1");
        drive3.AddFile(file1);
        user.AddDrive(drive3);

        Assert.Equal(drive3, file1.Parent);
        Assert.Contains(file1, drive3.Children);
    }

// Drive -> FolderA
//              -> FolderB
//              -> FileA
    [Fact]
    public void GrantPermissionToFolderAlsoAppliesToItsChildren(){
        User user = InitUserData();
        User guest = new User(2,"Jane");
        var drive3 = new Drive(3,"Drive3");
        var folderA = new Folder(1,"FolderA");
        var folderB = new Folder(2,"FolderB");
        var fileA = new File(1,"FileA");
        drive3.AddFolder(folderA);
        folderA.AddFolder(folderB);
        folderA.AddFile(fileA);
        user.AddDrive(drive3);

        user.GrantPermission(guest, folderA, PermissionType.Reader);
        StorePermission? result = user.StorePermissions.FirstOrDefault
        (x => x.UserId == guest.Id && x.StoreId == folderB.Id);
        Assert.NotNull(result); //Guest has permission
        Assert.Equal(PermissionType.Reader, result.Permission);
    }
}


