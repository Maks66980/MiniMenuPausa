using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuPausa : MonoBehaviour
{
    /* referencia al panel de pausa */
    [SerializeField] private CanvasGroup panelPausa;//refencia al panel de pausa
    /*variable to control the duration of the fade effect*/
    [SerializeField] private float duracionFade = 0.25f;//duración del efecto de fade
    [SerializeField] private VolumenMusica volumenMusica;//referencia al script VolumenMusica para controlar el volumen de la música
    [SerializeField] private VolumenSFX volumenSFX;//referencia al script VolumenSFX para controlar el volumen de los efectos de sonido
    [SerializeField] private ResolucionMenu resolucion;//referencia al script ResolucionMenu para controlar la resolución de pantalla
    [SerializeField] private PantallaCompletaMenu pantalla;//referencia al script PantallaCompletaMenu para controlar el modo de pantalla completa
    [SerializeField] private GameObject panelConfirmacion;//referencia al panel de confirmación
    [SerializeField] private Button botonApply;//referencia al botón de aplicar cambios
    [SerializeField] private Button botonDefault;//referencia al botón de valores predeterminados
    [SerializeField] private Button botonSi;//referencia al botón de confirmar salida sin guardar cambios
    [SerializeField] private Button botonNo;//referencia al botón de cancelar salida sin guardar cambios
    private bool pausado;
    private Coroutine transicion;
    private Selectable[] controles;
    private bool[] estadosControles;

    private bool HayCambios => volumenMusica.HayCambios || volumenSFX.HayCambios ||
        resolucion.HayCambios || pantalla.HayCambios;

    private void Start()
    {
        Time.timeScale = 1f;
        panelPausa.alpha = 0f;
        panelPausa.interactable = false;
        panelPausa.blocksRaycasts = false;
        panelConfirmacion.SetActive(false);
        controles = panelPausa.GetComponentsInChildren<Selectable>(true);
        estadosControles = new bool[controles.Length];
        botonApply.onClick.AddListener(Aplicar);
        botonDefault.onClick.AddListener(ValoresPredeterminados);
        botonSi.onClick.AddListener(DescartarYSalir);
        botonNo.onClick.AddListener(CancelarSalida);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        { CambiarPausa(); }
    }

    public void CambiarPausa()
    {
        if (!pausado)
        {
            Descartar();
            EstablecerPausa(true);
        }
        else if (panelConfirmacion.activeSelf) { CancelarSalida(); }
        else if (HayCambios) { MostrarConfirmacion(); }
        else { EstablecerPausa(false); }
    }

    public void Aplicar()
    {
        volumenMusica.Aplicar();
        volumenSFX.Aplicar();
        pantalla.Aplicar();
        resolucion.Aplicar(pantalla.PantallaCompleta);
        PlayerPrefs.Save();
    }

    public void ValoresPredeterminados()
    {
        volumenMusica.ValoresPredeterminados();
        volumenSFX.ValoresPredeterminados();
        resolucion.ValoresPredeterminados();
        pantalla.ValoresPredeterminados();
    }

    private void Descartar()
    {
        volumenMusica.Descartar();
        volumenSFX.Descartar();
        resolucion.Descartar();
        pantalla.Descartar();
    }

    private void MostrarConfirmacion()
    {
        for (int i = 0; i < controles.Length; i++)
        {
            estadosControles[i] = controles[i].interactable;
            if (!controles[i].transform.IsChildOf(panelConfirmacion.transform))
            { controles[i].interactable = false; }
        }
        panelConfirmacion.SetActive(true);
        if (EventSystem.current != null)
        { EventSystem.current.SetSelectedGameObject(botonNo.gameObject); }
    }

    public void CancelarSalida()
    {
        panelConfirmacion.SetActive(false);
        for (int i = 0; i < controles.Length; i++)
        { controles[i].interactable = estadosControles[i]; }
        if (EventSystem.current != null)
        { EventSystem.current.SetSelectedGameObject(botonApply.gameObject); }
    }

    public void DescartarYSalir()
    {
        CancelarSalida();
        Descartar();
        EstablecerPausa(false);
    }

    private void EstablecerPausa(bool nuevaPausa)
    {
        /* cambia el estado de pausa */
        pausado = nuevaPausa;
        //menu de pausa invisible
        float destino = 0f;
        //menu de pausa visible
        if (pausado) { Time.timeScale = 0f; destino = 1f; }
        else { Time.timeScale = 1f; }
        panelPausa.interactable = pausado;
        panelPausa.blocksRaycasts = pausado;
        if (transicion != null) { StopCoroutine(transicion); }
        transicion = StartCoroutine(HacerFade(destino));
        if (!pausado && EventSystem.current != null)
        { EventSystem.current.SetSelectedGameObject(null); }
    }

    //Cambia la opacidad del panel de pausa para crear un efecto de transición suave
    private IEnumerator HacerFade(float destino)
    {
        float inicio = panelPausa.alpha;
        float tiempo = 0f;
        //mientras el tiempo transcurrido sea menor que la duración del fade
        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;
            panelPausa.alpha = Mathf.Lerp(inicio, destino, tiempo / duracionFade);
            //espera un frame antes de continuar con la siguiente iteración
            yield return null;
        }
        panelPausa.alpha = destino;
        transicion = null;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        if (botonApply != null) { botonApply.onClick.RemoveListener(Aplicar); }
        if (botonDefault != null) { botonDefault.onClick.RemoveListener(ValoresPredeterminados); }
        if (botonSi != null) { botonSi.onClick.RemoveListener(DescartarYSalir); }
        if (botonNo != null) { botonNo.onClick.RemoveListener(CancelarSalida); }
    }
}
