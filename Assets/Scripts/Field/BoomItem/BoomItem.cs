using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomItem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D boomObj;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform pos;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float speed = 1;
    private float baseGravity = 2;

    public void SetSprtie(string itemDataName)
    {
        ItemManager itemManager = ItemManager.Instance;
        if (ItemManager.IsItemType(itemDataName) == ItemType.equitment)
        {
            SetSprtie(itemManager.equipment_Items[itemDataName]);
        }
        else if (ItemManager.IsItemType(itemDataName) == ItemType.material)
        {
            SetSprtie(itemManager.material_Items[itemDataName]);
        }
    }

    public void SetSprtie(ItemData itemData)
    {
        Sprite sprite = itemData.itemSprite;
        Vector3 scale = itemData.size;
        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.localScale = scale;
    }

    public void AddForce(float power, float angle)
    {
        pos = null;
        float th = angle * Mathf.Deg2Rad;
        boomObj.gravityScale = baseGravity;
        boomObj.velocity = Vector2.zero;
        boomObj.AddForce(new Vector2(Mathf.Cos(th), Mathf.Sin(th)) * power, ForceMode2D.Impulse);
    }

    public void GetItem()
    {
        Stop();

        if(gameObject.activeSelf)
        {
            StartCoroutine("Cor_GetItem");
        }
    }

    public void Stop()
    {
        boomObj.velocity = Vector2.zero;
        boomObj.gravityScale = 0;
        pos = Player.Instance.transform;
    }

    private IEnumerator Cor_GetItem()
    {
        if (pos == null)
            yield return null;

        float pSpeed = speed;
        Vector3 target = pos.transform.position + new Vector3(offset.x, offset.y, 0);
        while (Vector3.Distance(transform.position, target) >= pSpeed * Time.deltaTime)
        {
            Vector3 dic = (target - transform.position).normalized;
            Vector3 move = dic * pSpeed * Time.deltaTime;
            transform.position += move;
            pSpeed *= 1.1f;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        pos = null;
        gameObject.SetActive(false);
    }
}
