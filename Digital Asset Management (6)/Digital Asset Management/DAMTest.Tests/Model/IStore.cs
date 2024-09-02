public interface IStore {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<IStore> Children { get; }
    public IStore Parent { get; set; }

    public void AddFolder(Folder folder);
    public void AddFile(File file);
    public void RemoveChild(IStore child);
}