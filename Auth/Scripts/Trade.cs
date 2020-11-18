using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Auth.Scripts
{
	public class Trade : MonoBehaviour
	{
		public static Trade instance;
		public GameObject tradeCanvas;

		public TextMeshProUGUI inventoryText;

		[HideInInspector] public List<ItemInstance> inventory;

		[HideInInspector] public List<CatalogItem> catalog;

		public UnityEvent onRefreshUI;

		private void Awake()
		{
			instance = this;
		}


		public void OnLoggedIn()
		{
			tradeCanvas.SetActive(true);

			onRefreshUI?.Invoke();
		}

		#region Trade UI

		public void GetInventory()
		{
			inventoryText.text = "";

			// Req to get player's inv
			var getInvRequest = new GetPlayerCombinedInfoRequest
			{
				PlayFabId = LoginRegister.instance.playFabId,
				InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
				{
					GetUserInventory = true
				}
			};

			PlayFabClientAPI.GetPlayerCombinedInfo(getInvRequest,
				result =>
				{
					inventory = result.InfoResultPayload.UserInventory;

					foreach (var item in inventory) inventoryText.text += item.DisplayName + ", ";

					Debug.Log("Set item");
				},
				error => Debug.Log(error.ErrorMessage)
			);
		}

		public void GetCatalog()
		{
			// Debug.Log("Got Catalog");
			GetCatalogItemsRequest itemsRequest = new GetCatalogItemsRequest
			{
				CatalogVersion = "PlayerItems"
			};

			PlayFabClientAPI.GetCatalogItems(itemsRequest,
				result =>
				{
					Debug.Log(result.ToJson());
					catalog = result.Catalog;
				},
				error => Debug.Log(error.ErrorMessage)
			);
			// Debug.Log("Got " + catalog.Count); //This serves no purpose as execution wraps back to GetCatalogItems
		}

		#endregion
	}
}