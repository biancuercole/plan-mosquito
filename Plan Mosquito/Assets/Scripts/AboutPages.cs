using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AboutPages : MonoBehaviour
{

    public float duracion = 0.4f;

    public Button Volver;

    public Button Siguiente;

    private int paginaActual = 0;

    void Start()
    {

        InicializarPaginas();
        ActualizarBotones();
    }

    private void InicializarPaginas()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform pagina = transform.GetChild(i);

            pagina.localPosition = Vector3.zero;
            pagina.localRotation = Quaternion.identity;
            pagina.localScale = Vector3.one;
        }
    }

    public void PasarPagina()
    {
        if (paginaActual == transform.childCount - 1) return;

        Transform paginaTop = transform.GetChild(transform.childCount - 1);

        paginaTop.DOLocalMoveX(1500f, duracion).SetEase(Ease.InOutCubic);
        paginaTop.DORotate(new Vector3(0, 0, 15f), duracion);

        paginaTop.DOScale(0.95f, duracion).OnComplete(() =>
        {
            paginaTop.SetSiblingIndex(0);
            ResetTransform(paginaTop);

            paginaActual++;
            ActualizarBotones();
        });
    }

    public void VolverPagina()
    {
        if (paginaActual == 0) return;

        Transform paginaBack = transform.GetChild(0);

        paginaBack.SetSiblingIndex(transform.childCount - 1);
        paginaBack.localPosition = new Vector3(600f, 0, 0);
        paginaBack.localRotation = Quaternion.Euler(0, 0, 15f);
        paginaBack.localScale = Vector3.one;

        paginaBack.DOLocalMoveX(0f, duracion).SetEase(Ease.OutBack);
        paginaBack.DORotate(Vector3.zero, duracion).OnComplete(() =>
        {
            paginaActual--;

            ActualizarBotones();
        });
    }

    private void ResetTransform(Transform page)
    {
        page.localPosition = Vector3.zero;
        page.localRotation = Quaternion.identity;
        page.localScale = Vector3.one;
    }

    private void ActualizarBotones()
    {
        Volver.interactable = paginaActual > 0;
        Siguiente.interactable = paginaActual < transform.childCount - 1;
    }
}
