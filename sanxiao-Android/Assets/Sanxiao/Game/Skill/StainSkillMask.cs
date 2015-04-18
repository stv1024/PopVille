using UnityEngine;
using Fairwood.Math;

namespace Assets.Sanxiao.Game.Skill
{
    /// <summary>
    /// 污渍技能的障碍遮罩
    /// </summary>
    public class StainSkillMask : MonoBehaviour
    {
        public Texture2D TxrStain;

        private Texture2D _txrMask;

        //public Material MatMask;

        const int size = 512;

        void Start()
        {
            _txrMask = new Texture2D(size, size, TextureFormat.RGBA32, false);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    _txrMask.SetPixel(x, y, new Color(0.5f, 0.5f, 0.5f, 0));
                }
            }

            var tarTra = new GameObject().transform;
            tarTra.localScale = new Vector3(size, size, 1);
            var srcTra = new GameObject().transform;

            for (int i = 0; i < 5; i++)
            {
                var center = new Vector2(Random.Range(size * 0.2f, size * 0.8f), Random.Range(size * 0.2f, size * 0.8f));
                var dir = Random.Range(0, 360);
                var scale = Random.Range(1f, 3f);
                srcTra.position = center;
                srcTra.eulerAngles = new Vector3(0, 0, dir);
                srcTra.localScale = (Vector2.one*128*scale).ToVector3(1);

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        var tarPos = new Vector3((x + 0.5f)/size, (y + 0.5f)/size);
                        var srcPos = srcTra.InverseTransformPoint(tarTra.TransformPoint(tarPos));
                        if (srcPos.x >= 0 && srcPos.x <= 1 && srcPos.y >= 0 && srcPos.y <= 1)
                        {
                            //var oriColor = _txrMask.GetPixel(x, y);
                            var addColor = TxrStain.GetPixelBilinear(srcPos.x, srcPos.y);
                            //var newColor = ori
                            if (addColor.a > 0.5f) _txrMask.SetPixel(x, y, addColor);
                            //_txrMask.SetPixel(x, y, Color.black);
                        }
                    }
                }
            }
            _txrMask.Apply(false);
            GetComponent<Renderer>().material.mainTexture = _txrMask;
        }

        //public void ProcessTouch(params )
        //{
        
        //}

        void Update()
        {
            var touching = false;
            var curUVTouPos = new Vector2();
            if (Input.GetMouseButton(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (GetComponent<Collider>().Raycast(ray, out hitInfo, 1000))
                {
                    touching = true;
                    curUVTouPos = hitInfo.textureCoord;
                }
            }
            if (!touching && Input.touchCount > 0)
            {
                var ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                RaycastHit hitInfo;
                if (GetComponent<Collider>().Raycast(ray, out hitInfo, 1000))
                {
                    touching = true;
                    curUVTouPos = hitInfo.textureCoord;
                }
            }
            if (touching)
            {
                var center = curUVTouPos*size;
                var radius = 10;
                for (int i = -radius; i <= radius; i++)
                {
                    for (int j = -radius; j <= radius; j++)
                    {
                        if (new Vector2(i, j) .sqrMagnitude > radius*radius) continue;
                        _txrMask.SetPixel((int) (center.x) + i, (int) (center.y) + j, new Color(0.5f, 0.5f, 0.5f, 0));
                    }
                }
                _txrMask.Apply(false);
            }
        }
    }
}