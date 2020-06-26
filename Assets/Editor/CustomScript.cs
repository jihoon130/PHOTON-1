using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomScript : EditorWindow
{
    
    [MenuItem("Setting/Charater")]
   static void Init()
    {
        CustomScript window = GetWindow<CustomScript>(typeof(Move));
        window.minSize = new Vector2(500, 500);
        window.minSize = new Vector2(500, 500);
        window.Show();
    }

    void OnGUI()
    {
        GameObject sel = Selection.activeGameObject;

        Move targetComp = sel.GetComponent<Move>();
        Create BulletComp = sel.GetComponent<Create>();

        if (targetComp != null)
        {
            GUILayout.Label("이동속도", EditorStyles.boldLabel);
            targetComp.MoveSpeed = EditorGUILayout.FloatField(targetComp.MoveSpeed);
        }

        if(BulletComp != null)
        {
            GUILayout.Label("총알 이동속도", EditorStyles.boldLabel);
            BulletComp.BulletSpeed = EditorGUILayout.FloatField(BulletComp.BulletSpeed);

            GUILayout.Label("총알 사거리", EditorStyles.boldLabel);
            BulletComp.BulletDir = EditorGUILayout.FloatField(BulletComp.BulletDir);
        }

        if (targetComp && BulletComp)
        {
            if (GUILayout.Button("초기화"))
            {
                targetComp.MoveSpeed = EditorGUILayout.FloatField(14f);
                BulletComp.BulletSpeed = EditorGUILayout.FloatField(50f);
                BulletComp.BulletDir = EditorGUILayout.FloatField(0.3f);
            }
        }
            
    }
}
