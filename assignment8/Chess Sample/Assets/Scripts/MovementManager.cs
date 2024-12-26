using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject effectPrefab;
    private Transform effectParent;
    private List<GameObject> currentEffects = new List<GameObject>();   // 현재 effect들을 저장할 리스트
    
    public void Initialize(GameManager gameManager, GameObject effectPrefab, Transform effectParent)
    {
        this.gameManager = gameManager;
        this.effectPrefab = effectPrefab;
        this.effectParent = effectParent;
    }

    private bool TryMove(Piece piece, (int, int) targetPos, MoveInfo moveInfo)
    {
        // moveInfo의 distance만큼 direction을 이동시키며 이동이 가능한지를 체크
        // 보드에 있는지, 다른 piece에 의해 막히는지 등을 체크
        // 폰에 대한 예외 처리를 적용
        // --- TODO ---
        if (!Utils.IsInBoard(targetPos))
            return false;

        bool is_target_reachable = false;
        for (int i = 1; i <= moveInfo.distance; ++i)
        {
            int x = piece.MyPos.Item1 + i * moveInfo.dirX;
            int y = piece.MyPos.Item2 + i * moveInfo.dirY;

            if (Utils.IsInBoard((x, y))) {
                Piece target_piece = gameManager.Pieces[x, y];
                if (target_piece != null) {
                    if (target_piece.PlayerDirection == piece.PlayerDirection)
                        break;
                    else if ((x, y) != targetPos)
                        break;
                }
            }

            if ((x, y) == targetPos) {
                is_target_reachable = true;
                break;
            }
        }
        if (!is_target_reachable)
            return false;

        if (piece is Pawn) {
            if (moveInfo.dirX == 0 && gameManager.Pieces[targetPos.Item1, targetPos.Item2] != null)
                return false;
            if (moveInfo.dirX != 0 && gameManager.Pieces[targetPos.Item1, targetPos.Item2] == null)
                return false;
        }

        return true;
        // ------
    }

    // 체크를 제외한 상황에서 가능한 움직임인지를 검증
    private bool IsValidMoveWithoutCheck(Piece piece, (int, int) targetPos)
    {
        if (!Utils.IsInBoard(targetPos) || targetPos == piece.MyPos) return false;

        foreach (var moveInfo in piece.GetMoves())
        {
            if (TryMove(piece, targetPos, moveInfo))
                return true;
        }
        
        return false;
    }

    // 체크를 포함한 상황에서 가능한 움직임인지를 검증
    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMoveWithoutCheck(piece, targetPos)) return false;

        // 체크 상태 검증을 위한 임시 이동
        var originalPiece = gameManager.Pieces[targetPos.Item1, targetPos.Item2];
        var originalPos = piece.MyPos;

        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = piece;
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = null;
        piece.MyPos = targetPos;

        bool isValid = !IsInCheck(piece.PlayerDirection);

        // 원상 복구
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = piece;
        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = originalPiece;
        piece.MyPos = originalPos;

        return isValid;
    }

    // 체크인지를 확인
    private bool IsInCheck(int playerDirection)
    {
        (int, int) kingPos = (-1, -1); // 왕의 위치
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var piece = gameManager.Pieces[x, y];
                if (piece is King && piece.PlayerDirection == playerDirection)
                {
                    kingPos = (x, y);
                    break;
                }
            }
            if (kingPos.Item1 != -1 && kingPos.Item2 != -1) break;
        }

        // 왕이 지금 체크 상태인지를 리턴
        // gameManager.Pieces에서 Piece들을 참조하여 움직임을 확인
        // --- TODO ---
        foreach (var piece in gameManager.Pieces) {
            if (piece != null && piece.PlayerDirection != playerDirection)
                if (IsValidMoveWithoutCheck(piece, kingPos))
                    return true;
        }
        return false;
        // ------
    }

    public void ShowPossibleMoves(Piece piece)
    {
        ClearEffects();

        // 가능한 움직임을 표시
        // IsValidMove를 사용
        // effectPrefab을 effectParent의 자식으로 생성하고 위치를 적절히 설정
        // currentEffects에 effectPrefab을 추가
        // --- TODO ---
        foreach (var move in piece.GetMoves()) {
            for (int i = 1; i <= move.distance; ++i) {
                int x = piece.MyPos.Item1 + i * move.dirX;
                int y = piece.MyPos.Item2 + i * move.dirY;
                if (IsValidMove(piece, (x, y))) {
                    GameObject effect_object = Instantiate(effectPrefab, effectParent);
                    effect_object.transform.position = Utils.ToRealPos((x, y));
                    currentEffects.Add(effect_object);
                }
            }
        }
        // ------
    }

    // 효과 비우기
    public void ClearEffects()
    {
        foreach (var effect in currentEffects)
        {
            if (effect != null) Destroy(effect);
        }
        currentEffects.Clear();
    }
}