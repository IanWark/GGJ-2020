using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private const string scoreStartString = "You successfully treated\n";
    private const string scoreEndString = "\nPatients";

    [SerializeField]
    private GameObject gameOverPanel = null;

    [SerializeField]
    private Text scoreText = null;

    [SerializeField]
    private Button retryButton = null;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        
        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameEnd -= OnGameEnd;
        retryButton.onClick.RemoveListener(OnRetryButtonClicked);
    }

    public void OnGameEnd(int patientsCured, int patientsTotal)
    {
        gameOverPanel.SetActive(true);

        scoreText.text = scoreStartString + patientsCured.ToString() + "/" + patientsTotal.ToString() +  scoreEndString;

        retryButton.onClick.AddListener(OnRetryButtonClicked);
    }

    private void OnRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
