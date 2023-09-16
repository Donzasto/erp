<script lang="ts">
	import { onMount } from 'svelte';

	onMount(async () => {
		getUsers();
	});

	let users: User[] = [];

	async function getUsers() {
		const response = await fetch('/api/users', {
			method: 'GET',
			headers: { Accept: 'application/json' }
		});

		if (response.ok === true) {
			users = await response.json();
		}
	}

	async function deleteUser(id: number) {
		const response = await fetch(`/api/users/${id}`, {
			method: 'DELETE',
			headers: { Accept: 'application/json' }
		});
		if (response.ok === true) {
			const user = await response.json();
			document.querySelector(`tr[data-rowid='${user.id}']`)!.remove();
		} else {
			const error = await response.json();
			console.log(error.message);
		}
	}
</script>

<div class="parent">
	<div>
		<table>
			<thead>
				<tr>
					<th>Login</th>
					<th>Password</th>
				</tr>
			</thead>
			<tbody>
				{#each users as note}
					<tr data-rowid={note.id}>
						<td>{note.login}</td>
						<td>{note.password}</td>
						<td><input type="submit" value="Delete" on:click={() => deleteUser(note.id)} /> </td>
					</tr>
				{/each}
			</tbody>
		</table>
	</div>
	<div>
		<form method="POST" action="/api/users">
			<p>
				<label for="login">Login</label><br />
				<input name="login" />
			</p>
			<p>
				<label for="password">Password</label><br />
				<input name="password" />
			</p>
			<input type="submit" value="Save" />
		</form>
	</div>
</div>

<style>
	.parent {
		display: grid;
		grid-template-columns: repeat(2, 1fr);
	}
</style>
