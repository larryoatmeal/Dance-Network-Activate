using System;
using System.Collections.Generic;

public class SongMeta
{
	bool local = true;
	string musicPath;
	string midiPath;
	string thumbnail;
	string background;
	string author;
	string album;

	public SongMeta (bool local, string musicPath, string midiPath, string thumbnail, string background, string author, string album)
	{
		this.local = local;
		this.musicPath = musicPath;
		this.midiPath = midiPath;
		this.thumbnail = thumbnail;
		this.background = background;
		this.author = author;
		this.album = album;
	}
	public override string ToString ()
	{
		return string.Format ("[SongMeta: local={0}, musicPath={1}, midiPath={2}, thumbnail={3}, background={4}, author={5}, album={6}]", local, musicPath, midiPath, thumbnail, background, author, album);
	}

	public static SongMeta parse(JSONObject songJson){

		string musicPath = songJson.GetField ("audio").str;
		string midiPath = songJson.GetField ("midi").str;
		string background = songJson.GetField ("background").str;
		string thumbnail = songJson.GetField ("thumbnail").str;
		string author = songJson.GetField ("composer").str;
		string album = songJson.GetField ("album").str;

		return new SongMeta(false, musicPath, midiPath, thumbnail, background, author, album);	
	}

	public static List<SongMeta> parseMultiple(JSONObject songJson){
		List<SongMeta> songMetas = new List<SongMeta>();

		foreach(JSONObject j in songJson.list){
			songMetas.Add (SongMeta.parse (j));
		}
			
		return songMetas;
	}

}


