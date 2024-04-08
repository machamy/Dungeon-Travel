using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    /// <summary>
    /// DOTween 써도 되는데 직접 구현 해봄. - macham
    /// </summary>
    public class TransparentObstacleUnit : MonoBehaviour
    {
        private MeshRenderer[] renderers;
        private bool isTransparent;

        private float time;
        private WaitForSeconds waitTime;
        public float waitDelay = 0.1f;

        private const float THRESHOLD_ALPHA = 0.25f;
        private const float THRESHOLD_MAX_TIMER = 0.5f;


        private Coroutine _becomingTransparent;
        private Coroutine _becomingOpaque;
        private Coroutine _cheker;

        private void Awake()
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
            waitTime = new WaitForSeconds(waitDelay);
            isTransparent = false;
        }


        private void Start()
        {

        }

        private void Update()
        {

        }

        public void DoTransparent()
        {
            // Debug.Log($"{isTransparent} , {_becomingTransparent} , {_becomingOpaque}");
            if (isTransparent) // 투명일경우 탈출
            {
                RunCheckTimer();
                return;
            }
            if (_becomingTransparent != null && isTranspareting) // 투명화 중일경우 탈출
            {
                return;
            }

            if (_becomingOpaque != null && isOpaquing) // 복구중이었다면 중단후 시작
            {
                StopCoroutine(_becomingOpaque);
            }

            isTransparent = true;
            _becomingTransparent = StartCoroutine(TransparentRoutine());
        }

        private bool isTranspareting = false;
        
        IEnumerator TransparentRoutine()
        {
            isTranspareting = true;
            SetTransparent();
            while (true)
            {
                bool isComplete = true;

                foreach (var meshRenderer in renderers)
                {
                    if (meshRenderer.material.color.a > THRESHOLD_ALPHA)
                        isComplete = false;

                    var material = meshRenderer.material;

                    Color c = material.color;
                    c.a -= Time.deltaTime;
                    material.color = c;
                }

                if (isComplete)
                {
                    RunCheckTimer();
                    break;
                }

                yield return null;
            }

            isTranspareting = false;
        }

        private bool isOpaquing = false;
        IEnumerator OpaqueRoutine()
        {
            isOpaquing = true;
            while (true)
            {
                bool isComplete = true;

                foreach (var meshRenderer in renderers)
                {
                    if (meshRenderer.material.color.a < 1.0f)
                        isComplete = false;

                    var material = meshRenderer.material;

                    Color c = material.color;
                    c.a += Time.deltaTime;
                    material.color = c;
                }

                if (isComplete) break;

                yield return null;
            }

            isTransparent = false;
            SetOpaque();
            isOpaquing = false;
        }

        void RunCheckTimer()
        {
            if (_cheker != null)
            {
                StopCoroutine(_cheker);
            }

            _cheker = StartCoroutine(CheckerRoutine());
        }

        IEnumerator CheckerRoutine()
        {
            time = 0;
            while(time < THRESHOLD_MAX_TIMER)
            {
                yield return waitTime;
                time += waitDelay;
            }
            _becomingOpaque = StartCoroutine(OpaqueRoutine());
        }



        #region 머터리얼 관련

        enum BlendMode
        {
            Opaque,
            // Cutout,
            // Fade, 
            Transparent
        }

        private void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                // case BlendMode.Cutout:
                //     material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                //     material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                //     material.SetInt("_ZWrite", 1);
                //     material.EnableKeyword("_ALPHATEST_ON");
                //     material.DisableKeyword("_ALPHABLEND_ON");
                //     material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                //     material.renderQueue = 2450;
                //     break;
                // case BlendMode.Fade:
                //     material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                //     material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                //     material.SetInt("_ZWrite", 0);
                //     material.DisableKeyword("_ALPHATEST_ON");
                //     material.EnableKeyword("_ALPHABLEND_ON");
                //     material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                //     material.renderQueue = 3000;
                //     break;
                case BlendMode.Transparent:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
            }
        }
    

    private void SetTransparent()
        {
            foreach (var renderer in renderers)
            {
                foreach (var mat in renderer.materials)
                {
                    SetupMaterialWithBlendMode(mat, BlendMode.Transparent);
                }
            }
        }

    private void SetOpaque()
        {
            foreach (var renderer in renderers)
            {
                foreach (var mat in renderer.materials)
                {
                    SetupMaterialWithBlendMode(mat, BlendMode.Opaque);
                }
            }
        }

    #endregion
        }
    }
