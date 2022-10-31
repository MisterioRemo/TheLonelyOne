namespace TheLonelyOne
{
  public interface IInteractable: IDataPersistence
  {
    void PreInteract();
    void Interact();
    void PostInteract();
  }
}
