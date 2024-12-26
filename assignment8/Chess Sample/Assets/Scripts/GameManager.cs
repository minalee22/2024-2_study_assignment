using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 프리팹들
    public GameObject TilePrefab;
    public GameObject[] PiecePrefabs;   // King, Queen, Bishop, Knight, Rook, Pawn 순
    public GameObject EffectPrefab;

    // 오브젝트의 parent들
    private Transform TileParent;
    private Transform PieceParent;
    private Transform EffectParent;
    
    private MovementManager movementManager;
    private UIManager uiManager;
    
    public int CurrentTurn = 1; // 현재 턴 1 - 백, 2 - 흑
    public Tile[,] Tiles = new Tile[Utils.FieldWidth, Utils.FieldHeight];   // Tile들
    public Piece[,] Pieces = new Piece[Utils.FieldWidth, Utils.FieldHeight];    // Piece들

    void Awake()
    {
        TileParent = GameObject.Find("TileParent").transform;
        PieceParent = GameObject.Find("PieceParent").transform;
        EffectParent = GameObject.Find("EffectParent").transform;
        
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        movementManager = gameObject.AddComponent<MovementManager>();
        movementManager.Initialize(this, EffectPrefab, EffectParent);
        
        InitializeBoard();
    }

    void InitializeBoard()
    {
        // 8x8로 타일들을 배치
        // TilePrefab을 TileParent의 자식으로 생성하고, 배치함
        // Tiles를 채움
        // --- TODO ---
        for (int x = 0; x < Tiles.GetLength(0); ++x) {
            for (int y = 0; y < Tiles.GetLength(1); ++y) {
                GameObject tile = Instantiate(TilePrefab, TileParent);
                tile.name = $"{(char)(x + 'a')}{y + 1}";

                Tiles[x, y] = tile.GetComponent<Tile>();
                Tiles[x, y].Set((x, y));
            }
        }
        // ------

        PlacePieces(1);
        PlacePieces(-1);

        uiManager.UpdateTurn(CurrentTurn);
    }

    void PlacePieces(int direction)
    {
        // PlacePiece를 사용하여 Piece들을 적절한 모양으로 배치
        // --- TODO ---
        int y = direction == 1 ? 0 : Utils.FieldHeight - 1;
        PlacePiece(0, (4, y), direction);
        PlacePiece(1, (3, y), direction);
        PlacePiece(2, (2, y), direction);
        PlacePiece(2, (5, y), direction);
        PlacePiece(3, (1, y), direction);
        PlacePiece(3, (6, y), direction);
        PlacePiece(4, (0, y), direction);
        PlacePiece(4, (7, y), direction);
        for (int x = 0; x < Utils.FieldWidth; ++x) {
            PlacePiece(5, (x, y + direction), direction);
        }
        // ------
    }

    Piece PlacePiece(int pieceType, (int, int) pos, int direction)
    {
        // Piece를 배치 후, initialize
        // PiecePrefabs의 원소를 사용하여 배치, PieceParent의 자식으로 생성
        // Pieces를 채움
        // 배치한 Piece를 리턴
        // --- TODO ---
        GameObject piece_object = Instantiate(PiecePrefabs[pieceType], PieceParent);

        int x = pos.Item1;
        int y = pos.Item2;

        Pieces[x, y] = piece_object.GetComponent<Piece>();
        Pieces[x, y].initialize((x, y), direction);
        piece_object.name = Pieces[x, y].GetComponent<SpriteRenderer>().sprite.name;

        return Pieces[x, y];
        // ------
    }

    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        return movementManager.IsValidMove(piece, targetPos);
    }

    public void ShowPossibleMoves(Piece piece)
    {
        movementManager.ShowPossibleMoves(piece);
    }

    public void ClearEffects()
    {
        movementManager.ClearEffects();
    }


    public void Move(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMove(piece, targetPos)) return;

        // 해당 위치에 다른 Piece가 있다면 삭제
        // Piece를 이동시킴
        // --- TODO ---
        Piece target_piece = Pieces[targetPos.Item1, targetPos.Item2];
        if (target_piece != null && target_piece != piece)
            Destroy(target_piece.gameObject);

        piece.MoveTo(targetPos);

        ChangeTurn();
        // ------
    }

    void ChangeTurn()
    {
        // 턴을 변경하고, UI에 표시
        // --- TODO ---
        CurrentTurn = CurrentTurn == 1 ? -1 : 1;
        uiManager.UpdateTurn(CurrentTurn);
        // ------
    }
}
