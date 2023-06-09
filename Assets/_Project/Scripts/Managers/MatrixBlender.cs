using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MatrixBlender : MonoBehaviour
{
    public float fov = 60f,
                 near = .3f,
                 far = 500f,
                 orthographicSize = 20f;

    private float aspect;
    private static Matrix4x4 ortho, perspective;
    private Camera cam;

    public void Setup()
    {
        cam = Camera.main;
        aspect = (float)Screen.width / (float)Screen.height;
        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
    }

    public Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }

    /// <summary>
    /// Coroutine per fare un lerp della proiezione della camera verso la matrice indicata
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dest"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            cam.projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration);
            yield return 1;
        }
        cam.projectionMatrix = dest;
    }

    /// <summary>
    /// Funzione per avviare il processo di cambio matrice
    /// </summary>
    /// <param name="targetMatrix"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration)
    {
        StopAllCoroutines();
        return StartCoroutine(LerpFromTo(cam.projectionMatrix, targetMatrix, duration));
    }

    /// <summary>
    /// riferimento alla matrice ortogonale
    /// </summary>
    public static Matrix4x4 OrthoMatrix
    {
        get { return ortho; }
    }

    /// <summary>
    /// Riferimento alla matrice prospettica
    /// </summary>
    public static Matrix4x4 PerspectiveMatrix
    {
        get { return perspective; }
    }
}
