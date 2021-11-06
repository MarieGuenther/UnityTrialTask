using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformation : MonoBehaviour
{
    private float _radius = 2f;
    private float _deformationStrength = 2f;

    private Mesh _mesh = default;
    private Vector3[] _originalVertices = default, _modifiedVertices = default;
    private bool _deformationDirection = true;


    private void Start()
    {
        _mesh = GetComponentInChildren<MeshFilter>().mesh;
        _originalVertices = _mesh.vertices;
        _modifiedVertices = new Vector3[_originalVertices.Length];
        for (int i = 0; i < _originalVertices.Length; i++)
        {
            _modifiedVertices[i] = _originalVertices[i];
        }
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentMode == GameManager.GameMode.Edit && GameManager.Instance.CurrentEditMode == GameManager.EditMode.Terraforming && Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            _deformationStrength = GameManager.Instance.DeformationStrength;
            _radius = GameManager.Instance.DeformationRadius;
            _deformationDirection = GameManager.Instance.DeformationDirection;

            for (int v = 0; v < _modifiedVertices.Length; v++)
            {
                Vector3 distance = _modifiedVertices[v] - hit.point;

                float smoothingFactor = 2f;
                float force = _deformationStrength / (1f + hit.point.sqrMagnitude);

                if (distance.sqrMagnitude < _radius)
                {
                    int direction = 1;
                    if (!_deformationDirection)
                        direction = -1;
                    _modifiedVertices[v] = _modifiedVertices[v] + (Vector3.up * force * direction) / smoothingFactor;
                }
            }

            RecalculateMesh();
        }
    }

    private void RecalculateMesh()
    {
        _mesh.vertices = _modifiedVertices;
        GetComponentInChildren<MeshCollider>().sharedMesh = _mesh;
        _mesh.RecalculateNormals();
    }

}
