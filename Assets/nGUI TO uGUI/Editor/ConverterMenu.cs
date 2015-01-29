﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class ConverterMenu : MonoBehaviour {
	[MenuItem ("nGUI TO uGUI/Convert/Selected")]
	static void OnConvertSelected () {
		GameObject theSelectedObject;
		GameObject theNewObject;
		GameObject thEventSystem;

		if (Selection.activeGameObject != null){
			theSelectedObject = Selection.activeGameObject;
			Debug.Log (theSelectedObject.name);

			theNewObject = (GameObject)Instantiate (theSelectedObject);
			theNewObject.name = theSelectedObject.name;
			theSelectedObject.SetActive(false);

			thEventSystem = (GameObject)Instantiate ((AssetDatabase.LoadAssetAtPath ("Assets/nGUI TO uGUI/Prefabs/EventSystem.prefab", typeof(GameObject))));
			thEventSystem.name = "EventSystem";


		}else{
			Debug.LogError ("<Color=red>NOTHING SELECTED</Color>, <Color=yellow>Please select something to convert</Color>");
		}
	}

	[MenuItem ("nGUI TO uGUI/Convert/Widget")]
	static void OnConvertWidget () {
		GameObject theSelectedWidget;
		GameObject theNewWidget;
		GameObject thEventSystem;
		Texture2D theTextureAtlas;

		if (Selection.activeGameObject != null){
			if(!Directory.Exists("Assets/CONVERSION_DATA")){
				AssetDatabase.CreateFolder ("Assets", "CONVERSION_DATA");
			}else{

			}
			theSelectedWidget = Selection.activeGameObject;
			theNewWidget = (GameObject)Instantiate (theSelectedWidget);
			theNewWidget.name = theSelectedWidget.name;
			theSelectedWidget.SetActive(false);

			if (theNewWidget.GetComponent<UISprite>()){
				UISprite theUISprite;
				Image theReplacedUISprite;
				Sprite theNewResourceSprite;

				theUISprite = theNewWidget.GetComponent<UISprite>();
				theReplacedUISprite = theNewWidget.gameObject.AddComponent<Image>();

				theReplacedUISprite.color = theUISprite.color;

				float theUISpriteWidth = theUISprite.atlas.GetSprite(theUISprite.spriteName).width;
				float theUISpriteHeight = theUISprite.atlas.GetSprite(theUISprite.spriteName).height;
				float theUISpriteX = theUISprite.atlas.GetSprite(theUISprite.spriteName).x;
				float theUISpriteY = theUISprite.atlas.GetSprite(theUISprite.spriteName).y;

				Rect theRectFromAtlas = new Rect (theUISpriteX, theUISpriteY, theUISpriteWidth, theUISpriteHeight);

				Debug.Log (theRectFromAtlas);
				DestroyImmediate (theNewWidget.GetComponent<UISprite>());

				theNewResourceSprite = new Sprite();
				theNewResourceSprite = 	Sprite.Create((Texture2D)theUISprite.atlas.texture, theRectFromAtlas, new Vector2(0.0f,1.0f), 100.0f);
				//theReplacedUISprite.sprite = theNewResourceSprite;
				AssetDatabase.CopyAsset (AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(theUISprite.atlas.name)[0]), "Assets/CONVERSION_DATA/"+theUISprite.atlas.name+".png");
				AssetDatabase.Refresh();
				Debug.Log(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(theUISprite.atlas.name)[0]) + "\n" + "Assets/CONVERSION_DATA/"+theUISprite.atlas.name+".png");
				///

				string path = "Assets/CONVERSION_DATA/"+theUISprite.atlas.name+".png";
				TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
				importer.textureType = TextureImporterType.Sprite;
				importer.mipmapEnabled = false;
				importer.spriteImportMode = SpriteImportMode.Multiple;

				List <UISpriteData> theNGUISpritesList = theUISprite.atlas.spriteList;
				SpriteMetaData[] theSheet = new SpriteMetaData[theNGUISpritesList.Count];

				for (int c=0; c<theNGUISpritesList.Count; c++){
					float theY = theUISprite.atlas.texture.height - (theNGUISpritesList[c].y + theNGUISpritesList[c].height);
					theSheet[c].name = theNGUISpritesList[c].name;
					theSheet[c].pivot = new Vector2(theNGUISpritesList[c].paddingLeft, theNGUISpritesList[c].paddingBottom);
					theSheet[c].rect = new Rect (theNGUISpritesList[c].x, theY, theNGUISpritesList[c].width, theNGUISpritesList[c].height);
					theSheet[c].border = new Vector4(theNGUISpritesList[c].borderLeft, theNGUISpritesList[c].borderBottom, theNGUISpritesList[c].borderRight, theNGUISpritesList[c].borderTop);
					theSheet[c].alignment = 0;
					Debug.Log (theSheet[c].name + "       " + theSheet[c].pivot);
				}
				importer.spritesheet = theSheet;
				AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

				//theReplacedUISprite.sprite = (Sprite)AssetDatabase.LoadAssetAtPath ("Assets/CONVERSION_DATA/"+theUISprite.spriteName+".png", typeof(Sprite));


			}
		}else{
			Debug.LogError ("<Color=red>NOTHING SELECTED</Color>, <Color=yellow>Please select something to convert</Color>");
		}
	}


	/*
	[MenuItem ("nGUI TO uGUI/Convert/Scene")]
	static void OnConvertScene () {
		Debug.Log ("Doing Something...");
	}

	[MenuItem ("nGUI TO uGUI/Convert/Project")]
	static void OnConvertProject () {
		Debug.Log ("Doing Something...");
	}
	*/
}