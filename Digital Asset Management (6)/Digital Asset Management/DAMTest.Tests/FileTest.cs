namespace DAMTest.Tests;
public class FildeTest
{
    [Fact]
    public void FileCannotAddFile()
    {
        var file1 = new File(1, "File1");
        Assert.Throws<NotSupportedException>(() => file1.AddFile(new File(2, "File2")));
    }

    [Fact]
    public void FileCannotAddFolder()
    {
        var file1 = new File(1, "File1");
        Assert.Throws<NotSupportedException>(() => file1.AddFolder(new Folder(1, "Folder1")));
    }

    [Fact]
    public void FileCannotRemoveChild()
    {
        var file1 = new File(1, "File1");
        Assert.Throws<NotSupportedException>(() => file1.RemoveChild(new Folder(1, "Folder1")));
    }
}