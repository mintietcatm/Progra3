using UnityEngine;

public class PanelLogIn : MonoBehaviour
{
    
   public GameObject LogIn;
    public GameObject CreateAcc;

  
    public void Login()
    {
        LogIn.SetActive(true);
        CreateAcc.SetActive(false);
    }

    public void CrearCuenta()
    {
        CreateAcc.SetActive(true);
        CreateAcc.SetActive(false);
    }
}
