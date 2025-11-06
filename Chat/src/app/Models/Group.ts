import Message from "./Message";
import { UserHub } from "./UserHub";

export default interface Group {
  id?: number;
  name?: string | null;
  icon?: string;
  createdBy?: number | null;
  guid?: string;
  users?: UserHub[]
  messages?:Message[]
}
