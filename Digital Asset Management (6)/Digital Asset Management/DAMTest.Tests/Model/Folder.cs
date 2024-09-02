public class Folder : IStore
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<IStore> Children { get; } = new List<IStore>();
    public IStore Parent { get; set; }
    public Folder(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AddFile(File file)
    {
        if (Children.Contains(file))
        {
            throw new InvalidOperationException("Drive already owns this file.");
        }
        file.Parent = this;
        Children.Add(file);
    }

    public void AddFolder(Folder subFolder)
    {
        if (Children.Contains(subFolder))
        {
            throw new InvalidOperationException("Folder already owns this folder.");
        }
        subFolder.Parent = this;
        Children.Add(subFolder);
    }

    public void RemoveChild(IStore child)
    {
        if (!Children.Contains(child))
        {
            throw new InvalidOperationException("Folder does not own this child.");
        }
        Children.Remove(child);
    }
}