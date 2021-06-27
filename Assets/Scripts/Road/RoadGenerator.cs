using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _roadPiecesPrefabs;

    [SerializeField]
    private int _advancePieces = 6;

    [SerializeField]
    private Road _firstPiece;

    private Road[] _previousPieces;
    private Road[] _oldPieces;

    public void GenerateNextPieces(Road pCurrentRoad)
    {

        if(pCurrentRoad != _firstPiece && _firstPiece != null)
        {
            Destroy(_firstPiece.gameObject);
        }

        Road[] roadPieces = new Road[_advancePieces];

        for(int i = 0; i < roadPieces.Length; i++)
        {
            Road previousRoad = pCurrentRoad;

            if (_previousPieces != null && _previousPieces.Length > 0)
            {
                previousRoad = _previousPieces[_previousPieces.Length - 1];
            }

            if (i > 0)
            {
                previousRoad = roadPieces[i - 1];
            }

            GameObject piece = _roadPiecesPrefabs[0];

            int randomPiece = Random.Range(0, 12);

            if(randomPiece > 10)
            {
                piece = _roadPiecesPrefabs[1];
            }
            else if(randomPiece > 9)
            {
                piece = _roadPiecesPrefabs[2];
            }

            GameObject roadPiece = Instantiate(piece, transform);

            roadPiece.transform.position = previousRoad.End.position;

            switch (previousRoad.Direction)
            {
                case Road.Directions.Forward:
                    roadPiece.transform.rotation = previousRoad.transform.rotation;
                    break;

                case Road.Directions.Left:
                    roadPiece.transform.rotation = Quaternion.Euler(new Vector3(previousRoad.transform.rotation.eulerAngles.x, previousRoad.transform.rotation.eulerAngles.y + -90f, previousRoad.transform.rotation.eulerAngles.z));
                    break;

                case Road.Directions.Right:
                    roadPiece.transform.rotation = Quaternion.Euler(new Vector3(previousRoad.transform.rotation.eulerAngles.x, previousRoad.transform.rotation.eulerAngles.y + 90f, previousRoad.transform.rotation.eulerAngles.z));
                    break;
            }

            roadPieces[i] = roadPiece.GetComponent<Road>();

            roadPieces[i].RoadGenerator = this;

            if (i > 0)
            {
                roadPieces[i].NextPieceTrigger.SetActive(false);
            }
        }

        DeleteOldPieces();

        _oldPieces = _previousPieces;

        _previousPieces = roadPieces;
    }

    private void DeleteOldPieces()
    {
        if(_oldPieces != null && _oldPieces.Length > 0)
        {
            foreach(Road piece in _oldPieces)
            {
                Destroy(piece.gameObject);
            }
        }
    }
}
