import { Router } from "express";

import Playlist from "../models/Playlist";
import authorize from "../middleware/auth";

var router = Router();

router.get("/", authorize, async (req, res) => {
    const playlists = await Playlist.find({ owner: req.user });
    res.json(playlists);
});

router.post("/", authorize, async (req, res) => {
    const { name, songs } = req.body;

    const playlist = new Playlist({
        name,
        songs,
        owner: req.user,
    });
    await playlist.save();

    return res.json(playlist);
});

export default router;
