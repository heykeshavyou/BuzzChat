export default interface Message {
  id?: number;
  content?: string;
  sentOn?: string; // DateTime in C# → string (ISO format) in TypeScript
  fromId?: number;
  isRead?: boolean;
  toId?: number|null;
  groupId?: number | null; // nullable int → optional or null
}
