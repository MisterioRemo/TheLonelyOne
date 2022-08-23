namespace TheLonelyOne
{
  public interface IDataPersistence
  {
    void Save(ref GameData _gameData);
    void Load(GameData _gameData);
  }
}
