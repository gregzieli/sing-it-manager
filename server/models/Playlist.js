import mongoose from "mongoose";

import SongSchema from "./Song";

const PlaylistSchema = new mongoose.Schema(
    {
        name: {
            type: String,
            unique: true,
            required: true,
        },
        owner: {
            type: mongoose.Schema.Types.ObjectId,
            required: true,
        },
        songs: [SongSchema],
    },
    { timestamps: true }
);

export default mongoose.model("playlist", PlaylistSchema);

