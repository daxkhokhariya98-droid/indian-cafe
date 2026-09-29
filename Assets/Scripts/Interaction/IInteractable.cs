public interface IInteractable
{
    string InteractionPrompt { get; }
    void Interact(PlayerInteractor interactor);
    void OnHoverEnter();
    void OnHoverExit();
}