
public class Drive : IStore
{
    public int Id { get; set; }
    public string Name { get; set; }
   public List<IStore> Children { get; } = new List<IStore>();
    public IStore Parent { get; set; } = null;

    public Drive(int id, string name)
    {
         Id = id;
         Name = name;
    }

    public void AddFile(File file)
    {
        file.Parent = this;
        Children.Add(file);
    }

    public void AddFolder(Folder folder)
    {
        folder.Parent = this;
        Children.Add(folder);
    }

    public void RemoveChild(IStore store)
    {
        if (!Children.Contains(store))
        {
            throw new InvalidOperationException("Drive does not own this file.");
        }
        Children.Remove(store);
    }

  

    
}
