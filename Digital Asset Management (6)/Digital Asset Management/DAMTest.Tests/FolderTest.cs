namespace DAMTest.Tests;
public class FolderTest
{
     [Fact]
    public void Folder_AddFile(){
        var folder1 = new Folder(1,"Folder1");
        folder1.AddFile(new File(1,"File1"));
        folder1.AddFile(new File(2,"File2"));
        Assert.Equal(2, folder1.Children.Count);
    }

    [Fact]
    public void Folder_AddFolder(){
        var folder1 = new Folder(1,"Folder1");
        folder1.AddFolder(new Folder(1,"SubFolder1"));
        folder1.AddFolder(new Folder(2,"SubFolder2"));
        Assert.Equal(2, folder1.Children.Count);
    }

    [Fact]
    public void OneFolderCannotHaveSameFileTwice(){        
        var folder1 = new Folder(1,"Folder1");
        var file1 = new File(1,"File1");
        folder1.AddFile(file1);
        Assert.Throws<InvalidOperationException>(() => folder1.AddFile(file1));
    }

    [Fact]
    public void OneFolderCannotHaveSameSubFolderTwice(){        
        var folder1 = new Folder(1,"Folder1");
        var subFolder1 = new Folder(1,"SubFolder1");
        folder1.AddFile(new File(1,"File1"));
        folder1.AddFile(new File(2,"File2"));
        folder1.AddFolder(subFolder1);
        Assert.Throws<InvalidOperationException>(() => folder1.AddFolder(subFolder1));
    }

    [Fact]
    public void Folder_RemoveChild(){
        var folder1 = new Folder(1,"Folder1");
        var subFolder1 = new Folder(1,"SubFolder1");
        folder1.AddFolder(subFolder1);
        folder1.RemoveChild(subFolder1);
        Assert.Empty(folder1.Children);
    }

}