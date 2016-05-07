using System;
using System.Collections.Generic;

public class SongMeta
{
	public bool local = true;
	public string musicPath;
	public string midiPath;
	public string thumbnail;
	public string background;
	public string author;
	public string album;
	public string name;

	public SongMeta (bool local, string name, string musicPath, string midiPath, string thumbnail, string background, string author, string album)
	{
		this.local = local;
		this.musicPath = musicPath;
		this.midiPath = midiPath;
		this.thumbnail = thumbnail;
		this.background = background;
		this.author = author;
		this.album = album;
		this.name = name;
	}
	public override string ToString ()
	{
		return string.Format ("[SongMeta: local={0}, musicPath={1}, midiPath={2}, thumbnail={3}, background={4}, author={5}, album={6}]", local, musicPath, midiPath, thumbnail, background, author, album);
	}

	public static SongMeta parse(JSONObject songJson){
		string name = songJson.GetString ("name");
		string musicPath = songJson.GetString ("audio");
		string midiPath = songJson.GetString ("midi");
		string background = songJson.GetString ("background");
		string thumbnail = songJson.GetString ("thumbnail");
		string author = songJson.GetString ("composer");
		string album = songJson.GetString ("album");
		bool local = songJson.GetBool ("local");
		return new SongMeta(local, name, musicPath, midiPath, thumbnail, background, author, album);	
	}

	public static List<SongMeta> parseMultiple(JSONObject songJson){
		List<SongMeta> songMetas = new List<SongMeta>();

		foreach(JSONObject j in songJson.list){
			songMetas.Add (SongMeta.parse (j));
		}
			
		return songMetas;
	}
	public static List<SongMeta> parseString(String songJsonString){

		return parseMultiple(new JSONObject(songJsonString));
	}

}


