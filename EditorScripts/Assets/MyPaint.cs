using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class MyPaint : EditorWindow
{
    private Color _paintColor = new Color(0,0,0);
    private Color _eraseColor = new Color(255,255,255);
    private Color[,] _colors = new Color[12, 12];
    private GameObject _gameObject;
    private Texture _texture;
    private Texture2D _resultTexture;
    private MeshRenderer _meshRenderer;
    
    [MenuItem("Paint/MyPaint")]
    private static void OpenPaint()
    {
        GetWindow<MyPaint>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                _colors[i, j] = new Color(Random.value, Random.value, Random.value);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(200f));

        _paintColor = EditorGUILayout.ColorField("Paint Color", _paintColor);
        _eraseColor = EditorGUILayout.ColorField("Erase Color", _eraseColor);
        if (GUILayout.Button("Fill All"))
        {
            FillAll();
        }
        
        GUILayout.Space(500f);
        
        _gameObject = (GameObject) EditorGUILayout.ObjectField("Output Renderer", _gameObject, typeof(GameObject));
        if (GUILayout.Button("Save to Object"))
        {
            SaveToObject();
        }
        
        Event currentEvent = Event.current;
        _texture = EditorGUIUtility.whiteTexture;

        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                Rect rect = new Rect(i * 50 + 220, j * 50, 50, 50);

                GUI.color = _colors[i, j];
                GUI.DrawTexture(rect, _texture);
                
                if (currentEvent.type == EventType.MouseDown)
                {
                    ChangeColor(currentEvent, rect, i, j);
                }
            }
        }
    }

    private void ChangeColor(Event currentEvent, Rect rect, int i, int j)
    {
        if (currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
        {
            _colors[i, j] = _paintColor;
            currentEvent.Use();
        }

        if (currentEvent.button == 1 && rect.Contains(currentEvent.mousePosition))
        {
            _colors[i, j] = _eraseColor;
            currentEvent.Use();
        }
    }

    private void SaveToObject()
    {
        _meshRenderer = _gameObject.GetComponent<MeshRenderer>();
        _resultTexture = new Texture2D(12,12);
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                _resultTexture.SetPixel(i,j,_colors[i,j]);
            }
         
        }
        _resultTexture.Apply();
        _resultTexture.filterMode = FilterMode.Point;
        _meshRenderer.sharedMaterial.mainTexture = _resultTexture;
    }

    private void FillAll()
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                _colors[i, j] = _paintColor;
            }
        }
    }
}
