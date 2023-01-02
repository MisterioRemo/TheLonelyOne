using Zenject;

namespace TheLonelyOne
{
  public abstract class DataPersistenceNotMono: IDataPersistence, IInitializable
  {
    [Inject] private SaveSystem saveSystem;

    public void Initialize()
    {
      saveSystem.AddDataPersistenceObject(this);
      saveSystem.LoadDataPersistenceObject(this);
    }

    public abstract void Load(GameData _gameData);
    public abstract void Save(ref GameData _gameData);
  }
}
