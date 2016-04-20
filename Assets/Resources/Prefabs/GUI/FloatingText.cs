using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {
    
     public Color color = new Color(0.8f, 0.8f, 0f, 1.0f);
     public float scroll = 0.05f;  // scrolling velocity
     public float duration = 1.5f; // time to die
     public float alpha;
     Material mat;

     void Start()
     {
         GUIText text = GetComponent<GUIText>();
         mat = text.material;
         //mat.color = color; // set text color


         alpha = 1;
     }
 
     void Update()
     {
         if (alpha>0)
         {
             transform.position = new Vector3(transform.position.x, transform.position.y + scroll * Time.deltaTime, transform.position.z); 
             alpha -= Time.deltaTime/duration;
             Color c = mat.color;
             c.a = alpha;
             mat.color = c;
         } 
         else 
         {
             Destroy(gameObject); // text vanished - destroy itself
         }
     }
}
