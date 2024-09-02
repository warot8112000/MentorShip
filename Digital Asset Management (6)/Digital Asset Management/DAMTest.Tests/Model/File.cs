public class File : IStore
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<IStore> Children { get; } = null;
    public IStore Parent { get; set; }

    public File(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AddFolder(Folder folder)
    {
        throw new NotSupportedException("Cannot add a Folder to a File.");
    }

    public void AddFile(File file)
    {
        throw new NotSupportedException("Cannot add a File to a File.");
    }

    public void RemoveChild(IStore child)
    {
        throw new NotSupportedException("Cannot remove a child from a File.");
    }
}