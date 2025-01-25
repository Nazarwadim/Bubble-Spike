using UnityEngine;

public class MultiLayerBackgroundScroller : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        public Renderer renderer;
        public float scrollSpeed = 0.5f;
        public Vector2 direction = Vector2.right;
        public float layerWidth;
        [HideInInspector] public GameObject leftCopy;
        [HideInInspector] public GameObject rightCopy;
    }
    public Layer[] layers;
    private Vector2[] offsets;

    void Start()
    {
        offsets = new Vector2[layers.Length];

        foreach (var layer in layers)
        {
            if (layer.renderer != null)
            {

                layer.layerWidth = layer.renderer.bounds.size.x;


                layer.leftCopy = Instantiate(layer.renderer.gameObject, layer.renderer.transform.parent);
                layer.rightCopy = Instantiate(layer.renderer.gameObject, layer.renderer.transform.parent);


                layer.leftCopy.transform.position = layer.renderer.transform.position - new Vector3(layer.layerWidth, 0, 0);
                layer.rightCopy.transform.position = layer.renderer.transform.position + new Vector3(layer.layerWidth, 0, 0);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].renderer != null)
            {

                offsets[i] += Time.deltaTime * layers[i].scrollSpeed * layers[i].direction;
                layers[i].renderer.material.SetTextureOffset("_MainTex", offsets[i]);


                if (layers[i].leftCopy != null && layers[i].rightCopy != null)
                {
                    layers[i].leftCopy.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offsets[i]);
                    layers[i].rightCopy.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offsets[i]);
                }


                float layerPositionX = layers[i].renderer.transform.position.x;
                if (layerPositionX <= -layers[i].layerWidth || layerPositionX >= layers[i].layerWidth)
                {
                    layers[i].renderer.transform.position = Vector3.zero;
                    layers[i].leftCopy.transform.position = layers[i].renderer.transform.position - new Vector3(layers[i].layerWidth, 0, 0);
                    layers[i].rightCopy.transform.position = layers[i].renderer.transform.position + new Vector3(layers[i].layerWidth, 0, 0);
                }
            }
        }
    }

}