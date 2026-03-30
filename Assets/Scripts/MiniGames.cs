using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGames : MonoBehaviour
{
    [SerializeField] TileInfoScript tscr;
    [SerializeField] GameObject MiniGamePanel;
    [SerializeField] GameObject FrontPanel;
    [SerializeField] GameObject BackPanel;
    [SerializeField] Image MiniGamesPanelSubImage; 
    public Sprite[] sprites = new Sprite[2]; 

    void Start()
    {
        MiniGamePanel.SetActive(false);
    }

    public void ShowPanel()
    {
        MiniGamePanel.SetActive(true);
        if(tscr.isFEorBE()=="Back"){
            SetSpriteBack();
            BackPanel.SetActive(true);
            FrontPanel.SetActive(false);
            }

         if(tscr.isFEorBE()=="Front")
         {
            SetSpriteFront();
            BackPanel.SetActive(false);
            FrontPanel.SetActive(true);
         }
    }

    public void SetSpriteFront()
    {
        MiniGamesPanelSubImage.sprite = sprites[1];
    }

    public void SetSpriteBack()
    {
        MiniGamesPanelSubImage.sprite = sprites[0];
    }

    public void HidePanel()
{
    MiniGamePanel.SetActive(false);
}
}