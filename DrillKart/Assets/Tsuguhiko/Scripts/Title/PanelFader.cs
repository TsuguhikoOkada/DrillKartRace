using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

/// <summary>
/// DOTweenを使用して、指定されたImageコンポーネントをフェードインまたはフェードアウトさせるクラス。
/// フェードインが終了したらこのオブジェクトを無効化。
/// </summary>
public class PanelFader : MonoBehaviour
{
    /// <summary>
    /// フェード対象のImageコンポーネント。
    /// </summary>
    [SerializeField] private Image _targetImage;

    /// <summary>
    /// フェードにかかる時間（秒）。
    /// </summary>
    [SerializeField] private float _fadeDuration = 2.0f;

    /// <summary>
    /// フェードの開始時の色。
    /// </summary>
    [SerializeField] private Color _startColor = Color.black;

    /// <summary>
    /// フェードの終了時の色。
    /// </summary>
    [SerializeField] private Color _endColor = Color.white;

    /// <summary>
    /// DOTweenを使用してフェード処理を開始する。
    /// フェードインが完了したらオブジェクトを無効化する。
    /// </summary>
    void Start()
    {
        if (_targetImage == null)
        {
            Debug.LogError("フェードイン/アウトで使うためのImageがアサインされていません");
            return;
        }

        // 初期色を設定
        _targetImage.color = _startColor;

        // DOTweenを使ってフェード処理を実行
        _targetImage.DOColor(_endColor, _fadeDuration)
            .OnComplete(() =>
            {
                // フェード完了後にオブジェクトを無効化
                gameObject.SetActive(false);
            });
    }
}