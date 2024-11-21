using UnityEngine;

public class OpenRepository : MonoBehaviour
{
    // URL do repositório
    [SerializeField] private string repositoryUrl = "https://github.com/dunkexpial/seriousgame";

    // Método chamado pelo botão
    public void OpenGitHubRepository()
    {
        Application.OpenURL(repositoryUrl);
    }
}
