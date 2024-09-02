namespace DAMTest.Tests;
public class DriveTest
{
    [Fact]
    public void Drive_AddFolder()
    {
        var drive1 = new Drive(1, "Drive1");
        drive1.AddFolder(new Folder(1, "Folder1"));
        drive1.AddFolder(new Folder(2, "Folder2"));
        Assert.Equal(2, drive1.Children.Count);
    }

    [Fact]
    public void Drive_AddFile()
    {
        var drive1 = new Drive(1, "Drive1");
        var folder1 = new Folder(1, "Folder1");
        folder1.AddFile(new File(1, "File1"));
        folder1.AddFile(new File(2, "File2"));
        drive1.AddFolder(folder1);
        Assert.Equal(2, folder1.Children.Count);
    }

    [Fact]
    public void Drive_RemoveChild()
    {
        var drive1 = new Drive(1, "Drive1");
        var folder1 = new Folder(1, "Folder1");
        drive1.AddFolder(folder1);
        drive1.RemoveChild(folder1);
        Assert.Empty(drive1.Children);
    }
}