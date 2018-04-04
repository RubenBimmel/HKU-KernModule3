using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlatformNavigator : NavigationSystem {

    // Cell used by A* algorithm
    private class Cell {
        public int x, y;
        public float g, h;
        public float f { get { return g + h; } }
        public Cell parent;
    }

    // List of all possible moves
    private enum Move {
        Walk,
        JumpOneTile,
        JumpTwoTiles,
        JumpThreeTiles,
        JumpFourTiles,
        DropTwoTiles,
        DropThreeTiles,
        FallDown,
        FallDiagonal
    }

    // Possible directions for moves
    private enum Direction {
        left = -1,
        right = 1
    }

    public ProceduralTiles.Level level;     // Reference to the level
    private Cell[] cells;                   // List of all cellse in the level
    private List<Cell> path;                // List with cells of last calculated path
    private int positionOnPath;             // Current position on path
    public bool pathIsActive { get { return path != null; } }
    public bool aStarSearchIsRunning = false;

    // Constructor
    public PlatformNavigator(SideScrollerPawn _pawn, ProceduralTiles.Level _level) : base(_pawn) {
        level = _level;
        pawn = _pawn;

        cells = new Cell[level.GetSize()[0] * level.GetSize()[1]];
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = new Cell();
            cells[i].x = i % level.GetSize()[0];
            cells[i].y = (i - cells[i].x) / level.GetSize()[0];
            cells[i].g = float.MaxValue;
            cells[i].h = float.MaxValue;
        }
    }

    // Reset all F values for cells
    public void ResetCells() {
        for (int i = 0; i < cells.Length; i++) {
            cells[i].g = float.MaxValue;
            cells[i].h = float.MaxValue;
        }
    }

    // Get direction on path
    public override Vector3 GetDirection() {
        if (pawn) {
            if (path == null) {
                return Vector3.zero;
            }

            Vector2 pawnPos = pawn.transform.position;

            float offset = pawn.GetVelocity().x >= 0 ? .2f : -.2f;
            Cell currentPos = GetCell(pawnPos + Vector2.right * offset);

            // Check if pawn is on path
            if (currentPos != null && path.Contains(currentPos)) {
                // If grounded, wait until pawn is at the center and bottom of the cell
                if (IsBlocked(currentPos.x, currentPos.y - 1)) {
                    if (Mathf.Abs(pawnPos.x - level.TransformCellPosition(currentPos.x, currentPos.y).x) < .1f && pawnPos.y < level.TransformCellPosition(currentPos.x, currentPos.y).y - .2f) {
                        // Update position on path
                        positionOnPath = path.IndexOf(currentPos);
                    }
                }
                else {
                    // Update position on path
                    if (currentPos.x != path[positionOnPath].x) {
                        positionOnPath = path.IndexOf(currentPos);
                    }
                }
            }

            // Check if pawn is at target
            if (positionOnPath == path.Count - 1) {
                Debug.Log("Target reached");
                path = null;
                positionOnPath = 0;
                return Vector3.zero;
            }

            try {
                // Set direction X based on the relative position towards the target cell
                float dX = level.TransformCellPosition(path[positionOnPath + 1].x, path[positionOnPath + 1].y).x - pawnPos.x;
                if (dX > .3f) dX = 1;
                if (dX < -.3f) dX = -1;

                // Set direction Y based on the height difference between the target cell and the last cell
                float dY = path[positionOnPath + 1].y - path[positionOnPath].y;
                if (dY > 0) {
                    // If the pawn is near the edge of the cell and is moving towards the cell, or if the cells next to the pawn are blocked, the pawn should jump
                    if (dX > 0 && 
                        ((pawnPos.x > level.TransformCellPosition(currentPos.x, currentPos.y).x + .2f && pawn.GetVelocity().x > .3f) || 
                        (IsBlocked(currentPos.x + 1, currentPos.y) || IsBlocked(currentPos.x + 1, currentPos.y + 1)))) {
                        dY = 1;
                    }
                    else if (dX < 0 && 
                        ((pawnPos.x < level.TransformCellPosition(currentPos.x, currentPos.y).x - .2f && pawn.GetVelocity().x < -.3f) || 
                        (IsBlocked(currentPos.x - 1, currentPos.y) || IsBlocked(currentPos.x - 1, currentPos.y + 1)))) {
                        dY = 1;
                    }
                    // Else the pawn should not jump
                    else {
                        dY = 0;
                    }
                }

                // Debug target cell
                Vector3 targetPosition = level.TransformCellPosition(path[positionOnPath + 1].x, path[positionOnPath + 1].y);
                Debug.DrawLine(targetPosition + new Vector3 (-.5f, -.5f), targetPosition + new Vector3(.5f, .5f));
                Debug.DrawLine(targetPosition + new Vector3(.5f, -.5f), targetPosition + new Vector3(-.5f, .5f));

                // Return direction
                return new Vector3(dX, dY) * 10;
            }
            catch (System.ArgumentOutOfRangeException e) {
                Debug.Log("Navigation has exceeded the path, navigation is stopped");
                path = null;
                positionOnPath = 0;
                return Vector3.zero;
            }   
        }
        return Vector3.zero;
    }

    // Get cell at coordinates
    private Cell GetCell(int x, int y) {
        if (IsValid(x, y)) {
            return cells[x + y * level.GetSize()[0]];
        }
        return null;
    }

    // Get cell at world space position
    private Cell GetCell(Vector2 worldPosition) {
        int[] pos = level.GetPositionInGrid(worldPosition);
        return GetCell(pos[0], pos[1]);
    }

    // Check if cell is within range
    private bool IsValid (Cell cell) {
        return IsValid(cell.x, cell.y);
    }

    // Check if coordinates are within range
    private bool IsValid(int x, int y) {
        return (x >= 0) &&
            (x < level.GetSize()[0]) &&
            (y >= 0) &&
            (y < level.GetSize()[1]);
    }

    // Check if cell is accessible
    private bool IsAccessible(Cell cell) {
        if (cell == null) {
            return false;
        }
        return !IsBlocked(cell.x, cell.y) && !IsBlocked(cell.x, cell.y + 1);
    }

    // Check if cell is blocked
    private bool IsBlocked(int x, int y) {
        if (!IsValid(x, y)) {
            return false;
        }
        return (level.GetCollision(x, y) == ProceduralTiles.CollisionType.Block);
    }

    // Check distance between cells
    private float Distance(Cell a, Cell b) {
        Vector2 aPos = new Vector2(a.x, a.y);
        Vector2 bPos = new Vector2(b.x, b.y);
        return Vector2.Distance(aPos, bPos);
    }


    // Check if the pawn is on track
    public bool IsOnPath () {
        if (pathIsActive) {
            Cell current = GetCell(pawn.transform.position);
            Cell previous = path[positionOnPath];
            Cell next = path[positionOnPath];
            return (Mathf.Clamp(current.x, previous.x, next.x) == current.x &&
                Mathf.Clamp(current.y, previous.y, next.y) == current.y);
        }
        return false;
    }

    // A Function to find the shortest path between a given source cell to a 
    // destination cell according to A* Search Algorithm
    public IEnumerator AStarSearch() {
        ResetCells();
        aStarSearchIsRunning = true;

        Cell src = GetCell(pawn.transform.position + Vector3.up * .5f);
        while (!IsBlocked(src.x, src.y - 1)) {
            src = GetCell(src.x, src.y - 1);
        }
        Cell dst = GetCell(target);

        src.g = 0;
        src.h = Vector2.Distance(pawn.transform.position, target);

        if (!IsAccessible(src) || !IsValid(src)) {
            Debug.LogWarning("Source is blocked");
            aStarSearchIsRunning = false;
            yield break;
        }

        if (!IsAccessible(dst) || !IsValid(dst)) {
            Debug.LogWarning("Destination is blocked");
            aStarSearchIsRunning = false;
            yield break;
        }

        if (!IsBlocked(dst.x, dst.y -1)) {
            Debug.LogWarning("Destination is in air");
            aStarSearchIsRunning = false;
            yield break;
        }

        if (src == dst) {
            Debug.LogWarning("Pawn is already at destination");
            aStarSearchIsRunning = false;
            yield break;
        }
        
        List<Cell> open = new List<Cell>();
        List<Cell> closed = new List<Cell>();

        open.Add(src);

        while (open.Count > 0) {
            Cell current = open[0];
            //Get cell with lowest f value
            foreach (Cell cell in open) {
                if (cell.f < current.f) {
                    current = cell;
                }
            }

            //Add current to closed list
            open.Remove(current);
            closed.Add(current);

            if (current == dst) {
                path = RetracePath(src, current);
                positionOnPath = 0;
                DebugPath();
                aStarSearchIsRunning = false;
                yield break;
            }

            Cell[] children = GetCellsInReach(current);

            for (int i = 0; i < children.Length; i++) {
                if (children[i] != null && !closed.Contains(children[i])) {
                    float g = current.g + Distance(current, children[i]);
                    if (g < children[i].g) {
                        children[i].g = g;
                        children[i].h = Distance(children[i], dst);
                        children[i].parent = current;
                        if (!open.Contains(children[i])) {
                            open.Add(children[i]);
                        }
                    }
                }
            }
        }
    }

    // Create list of cells that are in reach
    private Cell[] GetCellsInReach (Cell cell) {
        int moves = Enum.GetNames(typeof(Move)).Length;
        Cell[] neighbours = new Cell[moves * 2];
        for (int i = 0; i < moves; i++) {
            neighbours[i] = CheckMove(cell, (Move)i, Direction.left);
            neighbours[moves + i] = CheckMove(cell, (Move)i, Direction.right);
        }
        return neighbours;
    }

    // Check if a move is possible. Return target if true, else return null
    private Cell CheckMove(Cell cell, Move move, Direction direction) {
        int d = (int)direction;
        Cell target = null;
        Cell[] cellsToCheck = new Cell[0];
        switch (move) {
            case Move.Walk:
                // Check if grounded 
                if (!IsBlocked(cell.x, cell.y - 1)) {
                    return null;
                }
                target = GetCell(cell.x + d, cell.y);
                break;
            case Move.JumpOneTile:
                // Check if grounded 
                if (!IsBlocked(cell.x, cell.y - 1)) {
                    return null;
                }
                target = GetCell(cell.x + d, cell.y + 2);
                cellsToCheck = new Cell[] {
                    GetCell(cell.x, cell.y + 2),
                    GetCell(cell.x, cell.y + 3)
                };
                break;
            case Move.JumpTwoTiles:
                // Check if grounded 
                if (!IsBlocked(cell.x, cell.y - 1)) {
                    return null;
                }
                target = GetCell(cell.x + d * 2, cell.y + 2);
                cellsToCheck = new Cell[] {
                    GetCell(cell.x, cell.y + 2),
                    GetCell(cell.x + d, cell.y + 1),
                    GetCell(cell.x + d, cell.y + 2),
                    GetCell(cell.x + d, cell.y + 3)
                };
                break;
            case Move.JumpThreeTiles:
                // Check if grounded 
                if (!IsBlocked(cell.x, cell.y - 1)) {
                    return null;
                }
                target = GetCell(cell.x + d * 3, cell.y + 2);
                cellsToCheck = new Cell[] {
                    GetCell(cell.x, cell.y + 2),
                    GetCell(cell.x + d, cell.y + 1),
                    GetCell(cell.x + d, cell.y + 2),
                    GetCell(cell.x + d, cell.y + 3),
                    GetCell(cell.x + d * 2, cell.y + 1),
                    GetCell(cell.x + d * 2, cell.y + 2),
                    GetCell(cell.x + d * 2, cell.y + 3)
                };
                break;
            case Move.JumpFourTiles:
                // Check if grounded 
                if (!IsBlocked(cell.x, cell.y - 1)) {
                    return null;
                }
                target = GetCell(cell.x + d * 4, cell.y + 1);
                cellsToCheck = new Cell[] {
                    GetCell(cell.x, cell.y + 2),
                    GetCell(cell.x + d, cell.y + 1),
                    GetCell(cell.x + d, cell.y + 2),
                    GetCell(cell.x + d, cell.y + 3),
                    GetCell(cell.x + d * 2, cell.y + 1),
                    GetCell(cell.x + d * 2, cell.y + 2),
                    GetCell(cell.x + d * 2, cell.y + 3),
                    GetCell(cell.x + d * 3, cell.y + 1),
                    GetCell(cell.x + d * 3, cell.y + 2),
                    GetCell(cell.x + d * 3, cell.y + 3)
                };
                break;
            case Move.DropTwoTiles:
                // Check if grounded 
                if (!IsBlocked(cell.x, cell.y - 1)) {
                    return null;
                }
                target = GetCell(cell.x + d * 2, cell.y - 1);
                cellsToCheck = new Cell[] {
                    GetCell(cell.x + d, cell.y - 1),
                    GetCell(cell.x + d, cell.y),
                    GetCell(cell.x + d, cell.y + 1)
                };
                break;
            case Move.DropThreeTiles:
                // Check if grounded 
                if (!IsBlocked(cell.x, cell.y - 1)) {
                    return null;
                }
                target = GetCell(cell.x + d * 3, cell.y - 2);
                cellsToCheck = new Cell[] {
                    GetCell(cell.x + d, cell.y - 1),
                    GetCell(cell.x + d, cell.y),
                    GetCell(cell.x + d, cell.y + 1),
                    GetCell(cell.x + d * 2, cell.y - 2),
                    GetCell(cell.x + d * 2, cell.y - 1),
                    GetCell(cell.x + d * 2, cell.y),
                };
                break;
            case Move.FallDown:
                target = GetCell(cell.x, cell.y - 1);
                break;
            case Move.FallDiagonal:
                target = GetCell(cell.x + d, cell.y - 3);
                cellsToCheck = new Cell[] {
                    GetCell(cell.x, cell.y - 1),
                    GetCell(cell.x, cell.y - 2),
                    GetCell(cell.x, cell.y - 3),
                    GetCell(cell.x + d, cell.y - 1)
                };
                break;
        }

        //Check if target is accessible
        if (!IsAccessible(target)) {
            return null;
        }

        //Check if move is possible
        foreach (Cell c in cellsToCheck) {
            if (c != null && IsBlocked(c.x, c.y)) {
                return null;
            }
        }

        return target;
    }

    // Retrace the path and reorder them from start to target
    private List<Cell> RetracePath(Cell start, Cell End) {
        List<Cell> returnPath = new List<Cell>();
        Cell current = End;

        while (current != start) {
            returnPath.Add(current);
            current = current.parent;
        }

        returnPath.Add(start);

        returnPath.Reverse();
        return returnPath;
    }

    // Debug the found path
    private void DebugPath() {
        for (int i = 0; i < path.Count - 1; i++) {
            int[] offset = level.GetSize();
            offset[0] = offset[0] / 2;
            offset[1] = offset[1] / 2;

            Vector3 pos1 = new Vector2(path[i].x - offset[0] + .5f, path[i].y - offset[1] + .5f);
            Vector3 pos2 = new Vector2(path[i + 1].x - offset[0] + .5f, path[i + 1].y - offset[1] + .5f);

            Debug.DrawLine(pos1, pos2, Color.yellow, .5f);
        }
    }
}
