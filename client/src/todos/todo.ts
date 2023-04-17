export enum TodoItemStatus {
  NotStarted,
  InProgress,
  Completed,
}

export class Todo {
  id: number | undefined;
  name: string = "";
  priority: number = 0;
  status: TodoItemStatus = TodoItemStatus.NotStarted;

  constructor(initializer?: any) {
    if (!initializer) return;
    if (initializer.id) this.id = initializer.id;
    if (initializer.name) this.name = initializer.name;
    if (initializer.priority) this.priority = initializer.priority;
    if (initializer.status) this.status = initializer.status;
  }

  /** Explicit to JSON conversion required for proper handling of the enum field value. */
  toJSON(): {
    name: string;
    id: number | undefined;
    priority: number;
    status: number;
  } {
    return {
      name: this.name,
      id: this.id,
      priority: this.priority,
      status: Number(this.status),
    };
  }
}
