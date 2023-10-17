using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace UnityEditor {
    [CreateAssetMenu]
    public class MergeTiles : RuleTile<MergeTiles.Neighbor> {
        public bool customField;

        public class Neighbor{
            public const int FriendTile = 1;
            public const int Null = 2;

        }

        public override bool RuleMatch(int neighbor, TileBase tile) {
            switch (neighbor) {
                case Neighbor.FriendTile: return tile == this || HasFriendTile(tile);
                case Neighbor.Null: return tile == null;
            }
            return base.RuleMatch(neighbor, tile);
        }

        private bool HasFriendTile(TileBase tile) {
            if(tile == null) return false;
            if(tile != null) return true;
            return false;
        }

    }
}