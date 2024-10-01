using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform itemAnchor;

    private HashSet<Table> _closeTables = new HashSet<Table>();
    private Table _closestTable;
    private Item _item;

    private void Update()
    {
        if (_closestTable)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _closestTable.Interact(this, _item);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Table table) && _closeTables.Add(table))
        {
            Debug.Log("OnTriggerEnter");
            UpdateClosestTable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Table table) && _closeTables.Remove(table))
        {
            UpdateClosestTable();
        }
    }

    private void UpdateClosestTable()
    {
        if (_closestTable) _closestTable.StopHighlight();

        _closestTable = GetClosestTable();

        if (_closestTable) _closestTable.StartHighlight();
    }

    private Table GetClosestTable()
    {
        if (_closeTables.Count == 0) return null;

        Table closest = null;
        float minDistanceSquared = Mathf.Infinity;

        foreach (var table in _closeTables)
        {
            float distanceSquared = (transform.position - table.transform.position).sqrMagnitude;
            if (distanceSquared < minDistanceSquared)
            {
                closest = table;
                minDistanceSquared = distanceSquared;
            }
        }

        return closest;
    }

    public bool GrabItem(Item item)
    {
        if (_item) return false; // El jugador ya tiene un ítem
        _item = item;
        _item.transform.SetParent(itemAnchor, false);
        _item.transform.localPosition = Vector3.zero;
        return true;
    }

    public void DropItem()
    {
        if (_item)
        {
            _item.transform.SetParent(null);
            _item = null;
        }
    }
}

